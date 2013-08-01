using System;
using FirstFloor.ModernUI.Presentation;

namespace WorkflowWorklist.ViewModels
{
    public abstract class IterativeFunctionVm<T> : NotifyPropertyChanged where T : class 
    {
        private int? _iterations;
        public int? Iterations
        {
            get { return _iterations; }
            set
            {
                _iterations = value;
                OnPropertyChanged("Iterations");
            }
        }

        public abstract T InitialCondition { get; set; }

        public abstract Func<T,T> UpdateFunction { get; set; }

        public bool IsValid
        {
            get
            {
                return Iterations.HasValue && (InitialCondition != null) && (UpdateFunction != null);
            }
        }
    }
}
