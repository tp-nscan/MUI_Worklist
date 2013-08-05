using System.ComponentModel;

namespace WorkflowWorklist.ViewModels
{
    public interface IWorkItemControllerVm : INotifyPropertyChanged
    {
        ISubmitFunctionVm SubmitFunctionVm { get; }
        IWorkItemMonitorVm WorkItemMonitorVm { get; }
    }
}