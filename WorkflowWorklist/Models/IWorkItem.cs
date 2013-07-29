using System;
using System.Threading.Tasks;

namespace WorkflowWorklist.Models
{
    public interface IWorkItemInfo
    {
        Guid Guid { get; }
        string Name { get; }
        object Result { get; }
    }

    public interface IWorkItem : IWorkItemInfo
    {
        IObservable<IProgressEventArgs> OnProgressChanged { get; }
        Task<object> RunAsync();
        void Cancel();
    }
}