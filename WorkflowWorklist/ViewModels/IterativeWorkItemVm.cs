using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public abstract class IterativeWorkItemVm<T> : NotifyPropertyChanged where T : class
    {
        protected IterativeWorkItemVm(IWorklist worklist)
        {
            Worklist = worklist;
        }

        private IWorkItemMonitorVm _workItemMonitorVm;
        private IWorkItemMonitorVm WorkItemMonitorVm
        {
            get { return _workItemMonitorVm; }
        }

        IWorklist Worklist { get; set; }
    }
}
