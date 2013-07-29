using System;
using System.ComponentModel;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public enum WorkItemVmState
    {
        Cancelled = 0,
        Completed = 1,
        Error = 2,
        Running = 3,
        Scheduled = 4
    }

    public interface IWorkItemVm : INotifyPropertyChanged
    {
        ICommand Cancel { get; }
        bool Cancelled { get; }
        bool Completed { get; }
        Guid Guid { get; }
        bool HasError { get; }
        bool IsRunning { get; }
        string Message { get; set; }
        string Name { get; }
        string Result { get; }
        string Status { get; }
        bool WasRun { get; }
        WorkItemVmState WorkItemVmState { get; set; }
    }

    public static class WorkItemVm
    {
        public static IWorkItemVm Make(Guid guid, string name, WorkItemVmState workItemVmState, IWorklist worklist)
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
        public WorkItemVmImpl(Guid guid, string name, WorkItemVmState workItemVmState, IWorklist worklist)
        {
            _guid = guid;
            WorkItemVmState = workItemVmState;
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

            Message = e.Message;
            Result = (string)e.WorkItemInfo.Result;
            OnPropertyChanged("Result");
            UpdateVmState(e.WorklistEventType);
        }

        void UpdateVmState(WorklistEventType worklistEventType)
        {
            switch (worklistEventType)
            {
                case WorklistEventType.ItemCancelled:
                    WorkItemVmState = WorkItemVmState.Cancelled;
                    break;
                case WorklistEventType.ItemCompleted:
                    WorkItemVmState = WorkItemVmState.Completed;
                    break;
                case WorklistEventType.ItemError:
                    WorkItemVmState = WorkItemVmState.Error;
                    break;
                case WorklistEventType.ItemStarted:
                    WorkItemVmState = WorkItemVmState.Running;
                    break;
                case WorklistEventType.ItemScheduled:
                    WorkItemVmState = WorkItemVmState.Scheduled;
                    break;
                case WorklistEventType.ItemUpdated:
                    WorkItemVmState = WorkItemVmState.Running;
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

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if(_message == value) return;
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        public string Result { get; private set; }

        public bool Cancelled
        {
            get { return WorkItemVmState == WorkItemVmState.Cancelled; }
        }

        public bool Completed
        {
            get { return WorkItemVmState == WorkItemVmState.Completed; }
        }

        public bool IsRunning
        {
            get { return WorkItemVmState == WorkItemVmState.Running; }
        }

        public bool HasError
        {
            get { return WorkItemVmState == WorkItemVmState.Error; }
        }

        private WorkItemVmState _workItemVmState;
        public WorkItemVmState WorkItemVmState
        {
            get { return _workItemVmState; }
            set
            {
                if (_workItemVmState == value) return;
                _workItemVmState = value;
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
            get { return WorkItemVmState.ToString(); }
        }

        public bool WasRun
        {
            get
            {
                return
                    (WorkItemVmState == WorkItemVmState.Error)
                    ||
                    (WorkItemVmState == WorkItemVmState.Cancelled)
                    ||
                    (WorkItemVmState == WorkItemVmState.Completed);
            }
        }

        private ICommand _camcel;
        public ICommand Cancel
        {
            get
            {
                return _camcel ?? (_camcel = new RelayCommand
                        (
                            o => Worklist.CancelCurrent(),
                            o => IsRunning
                        )
                    );
            }
        }
    }
}
