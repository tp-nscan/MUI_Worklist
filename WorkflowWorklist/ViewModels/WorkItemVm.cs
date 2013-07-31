using System;
using System.ComponentModel;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public interface IWorkItemVm : INotifyPropertyChanged
    {
        ICommand Cancel { get; }
        bool Cancelled { get; }
        bool Completed { get; }
        Guid Guid { get; }
        bool HasError { get; }
        bool IsRunning { get; }
        string Name { get; }
        string Result { get; }
        string Status { get; }
        bool WasRun { get; }
        WorkItemStatus WorkItemStatus { get; set; }
    }

    public static class WorkItemVm
    {
        public static IWorkItemVm Make(Guid guid, string name, WorkItemStatus workItemVmState, IWorklist worklist)
        {
            return new WorkItemVmImpl(guid, name, workItemVmState, worklist);
        }

        public static bool UnRunnable(this IWorkItemVm workItemVm)
        {
            return workItemVm.HasError || workItemVm.Completed || workItemVm.Cancelled;
        }
    }

    public class WorkItemVmImpl : NotifyPropertyChanged, IWorkItemVm
    {
        public WorkItemVmImpl(Guid guid, string name, WorkItemStatus workItemStatus, IWorklist worklist)
        {
            _guid = guid;
            WorkItemStatus = workItemStatus;
            _worklist = worklist;
            _name = name;
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

            Result = (string)e.WorkItemInfo.Result;
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

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        public string Result { get; private set; }

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
