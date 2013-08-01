using System;

namespace WorkflowWorklist.Models
{
    public interface IWorkItemInfo
    {
        Guid Guid { get; }
        string Name { get; }
        object Result { get; }
        int Step { get; }
        WorkItemStatus WorkItemStatus { get; }
    }

    public static class WorkItemInfo
    {
        public static IWorkItemInfo Make(Guid guid, string name, object result, int step, WorkItemStatus workItemStatus)
        {
            return new WorkItemInfoImpl(guid, name, result, step, workItemStatus);
        }

        private static readonly IWorkItemInfo empty = Make(Guid.Empty, string.Empty, null, 0, WorkItemStatus.None);

        public static IWorkItemInfo Empty
        {
            get
            {
                return empty;
            }
        }

        public static IWorkItemInfo ToWorkItemInfo(this IWorkItem workItem, object result, int step)
        {
            return Make(workItem.Guid, workItem.Name, result, step, workItem.WorkItemStatus);
        }
    }


    public class WorkItemInfoImpl : IWorkItemInfo
    {
        public WorkItemInfoImpl(Guid guid, string name, object result, int step, WorkItemStatus workItemStatus)
        {
            _guid = guid;
            _name = name;
            _result = result;
            _step = step;
            _workItemStatus = workItemStatus;
        }

        private readonly Guid _guid;
        private readonly string _name;
        private readonly object _result;
        private readonly int _step;
        private readonly WorkItemStatus _workItemStatus;

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

        public int Step
        {
            get { return _step; }
        }

        public WorkItemStatus WorkItemStatus
        {
            get { return _workItemStatus; }
        }
    }
}