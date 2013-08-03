using System;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using System.Reactive.Subjects;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public abstract class IterativeFunctionVm<T> : NotifyPropertyChanged where T : class 
    {
        protected IterativeFunctionVm()
        {
            IterativeFunction = new IterativeFunction<T>();
        }

        public Guid Guid
        {
            get { return IterativeFunction.Guid; }
        }

        IterativeFunction<T> IterativeFunction { get; set; }

        public int? Iterations
        {
            get { return IterativeFunction.Iterations; }
            set
            {
                IterativeFunction.Iterations = value;
                OnPropertyChanged("Iterations");
                OnPropertyChanged("IsValid");
            }
        }

        public T InitialCondition
        {
            get { return IterativeFunction.InitialCondition; }
            set
            {
                IterativeFunction.InitialCondition = value;
                OnPropertyChanged("InitialCondition");
                OnPropertyChanged("IsValid");
            }
        }

        public string Name
        {
            get { return IterativeFunction.Name; }
            set
            {
                IterativeFunction.Name = value;
                OnPropertyChanged("Message");
                OnPropertyChanged("IsValid");
            }
        }

        public Func<T, T> UpdateFunction
        {
            get { return IterativeFunction.UpdateFunction; }
            set
            {
                IterativeFunction.UpdateFunction = value;
                OnPropertyChanged("UpdateFunction");
                OnPropertyChanged("IsValid");
            }
        }

        public bool IsValid
        {
            get
            {
                return IterativeFunction.Iterations.HasValue 
                        && (IterativeFunction.InitialCondition != null)
                        && (! String.IsNullOrEmpty(IterativeFunction.Name)) 
                        && (IterativeFunction.UpdateFunction != null);
            }
        }

        readonly Subject<IterativeFunction<T>> _submitFunctionEvent = new Subject<IterativeFunction<T>>();
        public IObservable<IterativeFunction<T>> SubmitFunctionEvent { get { return _submitFunctionEvent; } }

        private ICommand _submit;
        public ICommand Submit
        {
            get
            {
                return _submit ?? (_submit = new RelayCommand
                        (
                            o => _submitFunctionEvent.OnNext(IterativeFunction),
                            o => IsValid
                        )
                    );
            }
        }
    }
}
