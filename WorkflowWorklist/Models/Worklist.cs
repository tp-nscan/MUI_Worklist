using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace WorkflowWorklist.Models
{
    [Export]
    public class Worklist : IDisposable, IWorklist
    {
        readonly Subject<WorklistEventArgs> _worklistEvent = new Subject<WorklistEventArgs>();
        public IObservable<WorklistEventArgs> OnWorklistEvent { get { return _worklistEvent; } }

        readonly ConcurrentQueue<IWorkItem> _queue;

        public Worklist()
        {
            _queue = new ConcurrentQueue<IWorkItem>();
        }

        public IEnumerable<IWorkItem> WorkItems
        {
            get
            {
                if (CurrentWorkItem != null)
                {
                    yield return CurrentWorkItem;
                }
                foreach (var workItem in _queue)
                {
                    yield return workItem;
                }
            }
        }

        public void PushIterative<T>
            (
                string name,
                Guid guid,
                T initialCondidtion, 
                Func<T, T> iterativeOp, 
                int iterations
            )
        {
            Push
                (
                    IterativeWorkItem.Make
                        (
                            name: name,
                            guid: guid,
                            initialConditon: initialCondidtion,
                            updateOperation: iterativeOp,
                            totalIterations: iterations
                        )
                );
        }

        public void CancelTask(Guid taskId)
        {
            if ((CurrentWorkItem != null) && (CurrentWorkItem.Guid == taskId))
            {
                CurrentWorkItem.Cancel();
                return;
            }

            foreach (var workItem in WorkItems)
            {
                if (workItem.Guid == taskId)
                {
                    workItem.Cancel();
                    _worklistEvent.OnNext(new WorklistEventArgs(this, workItem, WorklistEventType.ItemCancelled));
                }
            }
        }

        void Push(IWorkItem workItem)
        {
            _queue.Enqueue(workItem);
            _worklistEvent.OnNext(new WorklistEventArgs(this, workItem, WorklistEventType.ItemScheduled));
        }

        public void CancelAllTasks()
        {
            Stop();

            IWorkItem workItem;
            while (_queue.TryDequeue(out workItem))
            {
                _worklistEvent.OnNext(new WorklistEventArgs(this, workItem, WorklistEventType.ItemCancelled));
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public void Start()
        {
            if (IsRunning) return;

            _isRunning = true;
            QueueHandler();
        }

        public void Stop()
        {
            if (CurrentWorkItem != null)
            {
                CurrentWorkItem.Cancel();
            }
            _isRunning = false;
        }
        
        private IWorkItem _currentWorkItem;
        private IWorkItem CurrentWorkItem
        {
            get { return _currentWorkItem; }
        }

        async void QueueHandler()
        {
            _worklistEvent.OnNext(new WorklistEventArgs(this, WorkItemInfo.Empty, WorklistEventType.Started));

            do
            {
                if (!_queue.TryDequeue(out _currentWorkItem))
                {
                    _isRunning = false;
                    break;
                }

                CurrentWorkItem.OnProgressChanged.Subscribe
                    (
                        i =>
                            _worklistEvent.OnNext
                            (
                                new WorklistEventArgs
                                    (
                                        this,
                                        _currentWorkItem,
                                        WorklistEventType.ItemStarted
                                    )
                            )
                    );

                try
                {
                    if (CurrentWorkItem.WorkItemStatus != WorkItemStatus.Scheduled)
                    {
                        _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemCancelled));
                        continue;
                    }

                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemStarted));

                    await CurrentWorkItem.RunAsync();

                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemCompleted));
                }
                catch (TaskCanceledException)
                {
                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemCancelled));
                    continue;
                }
                catch (Exception)
                {
                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemError));
                }

                _currentWorkItem = null;

            } while (IsRunning);

            _worklistEvent.OnNext(new WorklistEventArgs(this, WorkItemInfo.Empty, WorklistEventType.Stopped));
        }

        public void Dispose()
        {
            CancelAllTasks();
        }
    }
}
