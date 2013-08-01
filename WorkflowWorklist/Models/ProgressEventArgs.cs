using System;

namespace WorkflowWorklist.Models
{
    public interface IProgressEventArgs
    {
        object Data { get; }
        string Message { get; }
        int Step { get; }
        Guid TaskId { get; }
    }

    public static class ProgressEventArgs 
    {
        public static IProgressEventArgs Create(Guid taskId, string message, object data, int step)
        {
            return new ProgressEventArgsImpl(taskId, message, data, step);
        }
    }

    class ProgressEventArgsImpl : EventArgs, IProgressEventArgs
    {
        public ProgressEventArgsImpl(Guid taskId, string message, object data, int step)
        {
            _taskId = taskId;
            _message = message;
            _data = data;
            _step = step;
        }

        private readonly Guid _taskId;
        public Guid TaskId
        {
            get { return _taskId; }
        }

        private readonly object _data;
        public object Data
        {
            get { return _data; }
        }

        private readonly string _message;
        public string Message
        {
            get { return _message; }
        }

        private readonly int _step;
        public int Step
        {
            get { return _step; }
        }
    }
}
