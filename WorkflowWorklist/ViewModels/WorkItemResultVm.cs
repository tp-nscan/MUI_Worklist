using System;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public interface IWorkItemResultVm : INotifyPropertyChanged
    {
        Guid Guid { get; }
        IWorklist Worklist { get; }
    }

    public abstract class WorkItemResultVm<T> : NotifyPropertyChanged, IWorkItemResultVm
    {
        protected WorkItemResultVm(IWorklist worklist, Guid guid)
        {
            _worklist = worklist;
            Worklist.OnWorklistEvent.Subscribe(Worklist_WorkListChanged);
            _guid = guid;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IWorklist _worklist;
        public IWorklist Worklist
        {
            get { return _worklist; }
        }

        void Worklist_WorkListChanged(WorklistEventArgs worklistEventArgs)
        {
            if (worklistEventArgs.WorkItemInfo.Guid != Guid)
            {
                return;
            }

            switch (worklistEventArgs.WorklistEventType)
            {
                case WorklistEventType.Started:
                    break;
                case WorklistEventType.Stopped:
                    break;
                case WorklistEventType.ItemCancelled:
                    break;
                case WorklistEventType.ItemCompleted:
                    ProcessResult((T)worklistEventArgs.WorkItemInfo.Result);
                    break;
                case WorklistEventType.ItemScheduled:
                    break;
                case WorklistEventType.ItemStarted:
                    break;
                case WorklistEventType.ItemUpdated:
                    ProcessResult((T) worklistEventArgs.WorkItemInfo.Result);
                    break;
            }
        }

        protected abstract void ProcessResult(T result);

    }
}
