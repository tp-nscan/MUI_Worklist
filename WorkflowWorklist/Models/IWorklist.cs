using System;

namespace WorkflowWorklist.Models
{
    public interface IWorklist
    {
        IObservable<WorklistEventArgs> OnWorklistEvent { get; }
        bool IsRunning { get; }

        void PushIterative<T>
            (
            string name,
            Guid guid,
            T initialCondidtion, 
            Func<T, T> iterativeOp, 
            int iterations
            );
        void Cancel(Guid taskId);
        void CancelAll();
        void Start();
        void Stop();
    }
}