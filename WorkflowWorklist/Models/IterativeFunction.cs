using System;

namespace WorkflowWorklist.Models
{
    public class IterativeFunction<T>
    {
        public IterativeFunction()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; private set; }

        public int? Iterations { get; set; }

        public T InitialCondition { get; set; }

        public string Name { get; set; }

        public Func<T, T> UpdateFunction { get; set; }
    }
}
