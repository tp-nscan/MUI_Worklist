namespace WorkflowWorklist.Models
{
    public class WorklistEventArgs
    {
        public WorklistEventArgs(IWorklist worklist, IWorkItemInfo workItemInfo, WorklistEventType worklistEventType)
        {
            _worklist = worklist;
            _worklistEventType = worklistEventType;
            _workItemInfo = workItemInfo;
        }

        private readonly WorklistEventType _worklistEventType;
        public WorklistEventType WorklistEventType
        {
            get { return _worklistEventType; }
        }

        private readonly IWorkItemInfo _workItemInfo;
        public IWorkItemInfo WorkItemInfo
        {
            get { return _workItemInfo; }
        }

        private readonly IWorklist _worklist;
        public IWorklist Worklist
        {
            get { return _worklist; }
        }
    }
}
