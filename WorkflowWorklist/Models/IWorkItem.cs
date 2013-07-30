using System;
using System.Threading.Tasks;

namespace WorkflowWorklist.Models
{
    public interface IWorkItemInfo
    {
        Guid Guid { get; }
        string Name { get; }
        object Result { get; }
        WorkItemStatus WorkItemStatus { get; }
    }

    public interface IWorkItem : IWorkItemInfo
    {
        IObservable<IProgressEventArgs> OnProgressChanged { get; }
        Task<object> RunAsync();
        void Cancel();
    }

    public class WorkItemInfo : IWorkItemInfo
    {
        static readonly WorkItemInfo empty = new WorkItemInfo
        {
            _guid = Guid.Empty, 
            _name = string.Empty, 
            _result = null,
            _workItemStatus = WorkItemStatus.None
        };

        public static IWorkItemInfo Empty
        {
            get
            {
                return empty;
            }
        }

        private Guid _guid;
        private string _name;
        private object _result;
        private WorkItemStatus _workItemStatus;

        public Guid Guid
        {
            get { return _guid; }
        }

        public string Name
        {
            get { return _name; }
        }

        public object Result
        {
            get { return _result; }
        }

        public WorkItemStatus WorkItemStatus
        {
            get { return _workItemStatus; }
        }
    }
}