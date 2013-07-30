namespace WorkflowWorklist.Models
{
    public enum WorklistEventType
    {
        ItemCancelled,
        ItemCompleted,
        ItemError,
        ItemScheduled,
        ItemStarted,
        ItemUpdated,
        Started,
        Stopped
    }
}