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

        private IWorkItemViewVm _workItemViewVm;
        private IWorkItemViewVm WorkItemViewVm
        {
            get { return _workItemViewVm; }
        }

        IWorklist Worklist { get; set; }
    }
}
