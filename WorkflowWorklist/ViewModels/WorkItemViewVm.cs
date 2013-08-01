using System;
using System.ComponentModel;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public interface IWorkItemViewVm : INotifyPropertyChanged
    {
        ICommand Cancel { get; }
        bool Cancelled { get; }
        bool Completed { get; }
        Guid Guid { get; }
        bool HasError { get; }
        bool IsRunning { get; }
        string Name { get; }
        IWorkItemInfo Result { get; }
        string Status { get; }
        bool WasRun { get; }
        WorkItemStatus WorkItemStatus { get; set; }
    }

    public static class WorkItemViewVm
    {
        public static IWorkItemViewVm Create(IWorklist worklist)
        {
            return new WorkItemViewViewVmImpl(worklist);
        }

        public static IWorkItemViewVm Schedule(Guid guid, string name, IWorklist worklist)
        {
            return new WorkItemViewViewVmImpl(guid, name, worklist);
        }

        public static bool UnRunnable(this IWorkItemViewVm workItemViewVm)
        {
            return workItemViewVm.HasError || workItemViewVm.Completed || workItemViewVm.Cancelled;
        }
    }

    public class WorkItemViewViewVmImpl : NotifyPropertyChanged, IWorkItemViewVm
    {
        public WorkItemViewViewVmImpl(Guid guid, string name, IWorklist worklist)
        {
            _guid = guid;
            WorkItemStatus = WorkItemStatus.Scheduled;
            _worklist = worklist;
            _name = name;
            Worklist.OnWorklistEvent.Subscribe(WorkListEventHandler);
        }

        public void SetTaskInfo(Guid guid, string name)
        {
            _guid = guid;
            _name = name;
        }

        public WorkItemViewViewVmImpl(IWorklist worklist)
        {
            WorkItemStatus = WorkItemStatus.None;
            _worklist = worklist;
            Worklist.OnWorklistEvent.Subscribe(WorkListEventHandler);
        }

        private readonly IWorklist _worklist;
        private IWorklist Worklist
        {
            get { return _worklist; }
        }

        void WorkListEventHandler(WorklistEventArgs e)
        {
            if (e.WorkItemInfo == null)
            {
                return;
            }
            if (e.WorkItemInfo.Guid != Guid)
            {
                return;
            }

            Result = e.WorkItemInfo;
            OnPropertyChanged("Result");
            UpdateVmState(e.WorklistEventType);
        }

        void UpdateVmState(WorklistEventType worklistEventType)
        {
            switch (worklistEventType)
            {
                case WorklistEventType.ItemCancelled:
                    WorkItemStatus = WorkItemStatus.Cancelled;
                    break;
                case WorklistEventType.ItemCompleted:
                    WorkItemStatus = WorkItemStatus.Completed;
                    break;
                case WorklistEventType.ItemError:
                    WorkItemStatus = WorkItemStatus.Error;
                    break;
                case WorklistEventType.ItemStarted:
                    WorkItemStatus = WorkItemStatus.Running;
                    break;
                case WorklistEventType.ItemScheduled:
                    WorkItemStatus = WorkItemStatus.Scheduled;
                    break;
                case WorklistEventType.ItemUpdated:
                    WorkItemStatus = WorkItemStatus.Running;
                    break;
                case WorklistEventType.Started:
                    break;
                case WorklistEventType.Stopped:
                    break;
            }
        }

        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
        }

        public IWorkItemInfo Result { get; private set; }

        public bool Cancelled
        {
            get { return WorkItemStatus == WorkItemStatus.Cancelled; }
        }

        public bool Completed
        {
            get { return WorkItemStatus == WorkItemStatus.Completed; }
        }

        public bool IsRunning
        {
            get { return WorkItemStatus == WorkItemStatus.Running; }
        }

        public bool HasError
        {
            get { return WorkItemStatus == WorkItemStatus.Error; }
        }

        private WorkItemStatus _workItemStatus;
        public WorkItemStatus WorkItemStatus
        {
            get { return _workItemStatus; }
            set
            {
                if (_workItemStatus == value) return;
                _workItemStatus = value;
                OnPropertyChanged("Cancelled");
                OnPropertyChanged("Completed");
                OnPropertyChanged("IsRunning");
                OnPropertyChanged("HasError");
                OnPropertyChanged("WasRun");
                OnPropertyChanged("Status");
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Status
        {
            get { return WorkItemStatus.ToString(); }
        }

        public bool WasRun
        {
            get
            {
                return
                    (WorkItemStatus == WorkItemStatus.Error)
                    ||
                    (WorkItemStatus == WorkItemStatus.Cancelled)
                    ||
                    (WorkItemStatus == WorkItemStatus.Completed);
            }
        }

        private ICommand _camcel;
        public ICommand Cancel
        {
            get
            {
                return _camcel ?? (_camcel = new RelayCommand
                        (
                            o => Worklist.CancelTask(Guid),
                            o => true
                        )
                    );
            }
        }
    }
}
