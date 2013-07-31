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

        public void Cancel(Guid taskId)
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
                    _worklistEvent.OnNext(new WorklistEventArgs(this, workItem, WorklistEventType.ItemCancelled, string.Empty));
                }
            }
        }

        void Push(IWorkItem workItem)
        {
            _queue.Enqueue(workItem);
            _worklistEvent.OnNext(new WorklistEventArgs(this, workItem, WorklistEventType.ItemScheduled, string.Empty));
        }

        public void CancelAll()
        {
            Stop();

            IWorkItem workItem;
            while (_queue.TryDequeue(out workItem))
            {
                _worklistEvent.OnNext(new WorklistEventArgs(this, workItem, WorklistEventType.ItemCancelled, string.Empty));
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
            _worklistEvent.OnNext(new WorklistEventArgs(this, WorkItemInfo.Empty, WorklistEventType.Started, string.Empty));

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
                                        WorklistEventType.ItemStarted,
                                        string.Empty
                                    )
                            )
                    );

                try
                {
                    if (CurrentWorkItem.WorkItemStatus != WorkItemStatus.Scheduled)
                    {
                        _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemCancelled, string.Empty));
                        continue;
                    }

                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemStarted, string.Empty));

                    await CurrentWorkItem.RunAsync();

                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemCompleted, string.Empty));
                }
                catch (TaskCanceledException)
                {
                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemCancelled, string.Empty));
                    continue;
                }
                catch (Exception)
                {
                    _worklistEvent.OnNext(new WorklistEventArgs(this, CurrentWorkItem, WorklistEventType.ItemError, string.Empty));
                }

                _currentWorkItem = null;

            } while (IsRunning);

            _worklistEvent.OnNext(new WorklistEventArgs(this, WorkItemInfo.Empty, WorklistEventType.Stopped, string.Empty));
        }

        public void Dispose()
        {
            CancelAll();
        }
    }
}
