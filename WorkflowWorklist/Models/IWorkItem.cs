using System;
using System.Threading.Tasks;

namespace WorkflowWorklist.Models
{
    public interface IWorkItem
    {
        IObservable<IProgressEventArgs> OnProgressChanged { get; }
        Task<object> RunAsync();
        void Cancel();
        Guid Guid { get; }
        string Name { get; }
        WorkItemStatus WorkItemStatus { get; }
    }

}