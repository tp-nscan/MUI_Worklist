using System;
using System.ComponentModel;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public interface IWorkItemMonitorVm : INotifyPropertyChanged
    {
        ICommand Cancel { get; }
        bool Cancelled { get; }
        bool Completed { get; }
        Guid Guid { get; }
        bool HasError { get; }
        bool IsRunning { get; }
        string Name { get; }
        string Message { get; }
        string Status { get; }
        bool WasRun { get; }
        WorkItemStatus WorkItemStatus { get; set; }
    }

    public static class WorkItemMonitorVm
    {
        public static IWorkItemMonitorVm Create(IWorklist worklist, Guid guid)
        {
            return new WorkItemMonitorVmImpl(guid, worklist);
        }

        public static IWorkItemMonitorVm Schedule(IWorkItemInfo workItemInfo, IWorklist worklist)
        {
            return new WorkItemMonitorVmImpl(workItemInfo, worklist);
        }

        public static bool UnRunnable(this IWorkItemMonitorVm workItemMonitorVm)
        {
            return workItemMonitorVm.HasError || workItemMonitorVm.Completed || workItemMonitorVm.Cancelled;
        }
    }

    public class WorkItemMonitorVmImpl : NotifyPropertyChanged, IWorkItemMonitorVm
    {
        public WorkItemMonitorVmImpl(Guid guid, IWorklist worklist)
        {
            _guid = guid;
            _worklist = worklist;
            WorkItemStatus = WorkItemStatus.None;
            Worklist.OnWorklistEvent.Subscribe(WorkListEventHandler);
        }

        public WorkItemMonitorVmImpl(IWorkItemInfo workItemInfo, IWorklist worklist)
        {
            _guid = workItemInfo.Guid;
            WorkItemStatus = workItemInfo.WorkItemStatus;
            _worklist = worklist;
            _name = workItemInfo.Name;
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

            Message = e.Message;
            WorkItemStatus = e.WorkItemInfo.WorkItemStatus;
            Name = e.WorkItemInfo.Name;
            OnPropertyChanged("Guid");
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            private set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

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
                OnPropertyChanged("Status");
                OnPropertyChanged("WasRun");
                OnPropertyChanged("WorkItemStatus");
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
