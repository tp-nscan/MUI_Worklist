namespace WorkflowWorklist.Models
{
    public enum WorkItemStatus
    {
        None        = 0,
        Cancelled   = 1,
        Completed   = 2,
        Error       = 3,
        Running     = 4,
        Scheduled   = 5
    }
}
