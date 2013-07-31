using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    //[InheritedExport]
    public interface IWorklistVm
    {
        int TaskCount { get; }
    }

    [Export("Worklist")]
    public class WorklistVm : NotifyPropertyChanged, IWorklistVm, IPartImportsSatisfiedNotification
    {
        public void OnImportsSatisfied()
        {
            Worklist.OnWorklistEvent.Subscribe(Worklist_WorkListChanged);
            foreach (var workItem in Worklist.WorkItems)
            {
                WorkItemVMs.Add(WorkItemVm.Make(workItem.Guid, workItem.Name, WorkItemStatus.Scheduled, Worklist));
            }
        }

        void Worklist_WorkListChanged(WorklistEventArgs worklistEventArgs)
        {
            switch (worklistEventArgs.WorklistEventType)
            {
                case WorklistEventType.Started:
                    Message = "Running";
                    break;
                case WorklistEventType.Stopped:
                    Message = "Stopped";
                    break;
                case WorklistEventType.ItemCancelled:
                    break;
                case WorklistEventType.ItemCompleted:
                    break;
                case WorklistEventType.ItemScheduled:
                    WorkItemVMs.Add
                        (
                            WorkItemVm.Make
                                (
                                    worklistEventArgs.WorkItemInfo.Guid, 
                                    worklistEventArgs.WorkItemInfo.Name,
                                    WorkItemStatus.Scheduled, 
                                    worklistEventArgs.Worklist
                                )
                        );
                    break;
                case WorklistEventType.ItemStarted:
                    break;
                case WorklistEventType.ItemUpdated:
                    break;
            }
        }

        [Import]
        private Worklist Worklist { get; set; }

        private ObservableCollection<IWorkItemVm> _workItemVMs;
        public ObservableCollection<IWorkItemVm> WorkItemVMs
        {
            get
            {
                if (_workItemVMs == null)
                {
                    _workItemVMs = new ObservableCollection<IWorkItemVm>();
                    _workItemVMs.CollectionChanged += (s, e) => OnPropertyChanged("TaskCount");
                }
                return _workItemVMs;
            }

            set { _workItemVMs = value; }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public int TaskCount
        {
            get { return WorkItemVMs.Count; }
        }

        private ICommand _add;
        public ICommand Add
        {
            get
            {
                return _add ?? (_add = new RelayCommand
                    (
                        o => Worklist.PushIterative
                        (
                            name: "name", 
                            guid: Guid.NewGuid(),
                            initialCondidtion: "hi", 
                            iterativeOp: s =>
                            {
                                Thread.Sleep(1000);
                                return s + "_next";
                            }, 
                            iterations: 4),
                        o => true
                    )
                );
            }
        }

        private ICommand _clean;
        public ICommand Clean
        {
            get
            {
                return _clean ?? (_clean = new RelayCommand
                    (
                        o => Worklist.CancelAll(),
                        OnCanExecute
                    )
                );
            }
        }

        private bool OnCanExecute(object o)
        {
            return (Worklist != null) && WorkItemVMs.Any();
        }

        private ICommand _clear;
        public ICommand Clear
        {
            get
            {
                return _clear ?? (_clear = new RelayCommand
                    (
                        o =>
                        {
                            var removals = WorkItemVMs.Where(T => T.UnRunnable()).ToList();
                            foreach (var workItemVm in removals)
                            {
                                WorkItemVMs.Remove(workItemVm);
                            }
                        },
                        o => (Worklist != null) &&  WorkItemVMs.Any(T => T.UnRunnable())
                    )
                );
            }
        }

        private ICommand _start;
        public ICommand Start
        {
            get
            {
                return _start ?? (_start = new RelayCommand
                        (
                            o => Worklist.Start(),
                            o => (Worklist != null) && (!Worklist.IsRunning) && (WorkItemVMs.Any(T => !T.UnRunnable()))
                        ));
            }
        }

        private ICommand _stop;
        public ICommand Stop
        {
            get
            {
                return _stop ?? (_stop = new RelayCommand
                        (
                            o => Worklist.Stop(),
                            o => (Worklist != null) && Worklist.IsRunning
                        ));
            }
        }
    }

}
