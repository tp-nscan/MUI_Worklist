using System;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public interface IWorklistResultVm : INotifyPropertyChanged
    {
        void SetWorklist(IWorklist worklist);
    }

    public abstract class WorklistResultVm<T> : NotifyPropertyChanged, IWorklistResultVm
    {
        public void SetWorklist(IWorklist worklist)
        {
            Worklst = worklist;
        }

        private IWorklist _worklst;
        private IWorklist Worklst
        {
            get { return _worklst; }
            set
            {
                _worklst = value;
                _worklst.OnWorklistEvent.Subscribe(Worklist_WorkListChanged);
            }
        }

        void Worklist_WorkListChanged(WorklistEventArgs worklistEventArgs)
        {
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
