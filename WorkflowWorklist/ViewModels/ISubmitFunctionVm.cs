using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WorkflowWorklist.ViewModels
{
    public interface ISubmitFunctionVm: INotifyPropertyChanged
    {
        Guid Guid { get; }
        string Name { get; set; }
        bool WasSubmitted { get; set; }
        bool CanSubmit { get; }
        ICommand Submit { get; }
    }
}