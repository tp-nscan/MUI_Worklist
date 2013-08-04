using System;
using System.ComponentModel;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using System.Reactive.Subjects;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public interface IIterativeFunctionVm<T> : INotifyPropertyChanged where T : class
    {
        Guid Guid { get; }
        int? Iterations { get; set; }
        T InitialCondition { get; set; }
        string Name { get; set; }
        Func<T, T> UpdateFunction { get; set; }
        bool WasSubmitted { get; set; }
        bool CanSubmit { get; }
        IObservable<IterativeFunction<T>> SubmitFunctionEvent { get; }
        ICommand Submit { get; }
    }

    public abstract class IterativeFunctionVm<T> : NotifyPropertyChanged, IIterativeFunctionVm<T> where T : class 
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
                OnPropertyChanged("CanSubmit");
            }
        }

        public T InitialCondition
        {
            get { return IterativeFunction.InitialCondition; }
            set
            {
                IterativeFunction.InitialCondition = value;
                OnPropertyChanged("InitialCondition");
                OnPropertyChanged("CanSubmit");
            }
        }

        public string Name
        {
            get { return IterativeFunction.Name; }
            set
            {
                IterativeFunction.Name = value;
                OnPropertyChanged("Message");
                OnPropertyChanged("CanSubmit");
            }
        }

        public Func<T, T> UpdateFunction
        {
            get { return IterativeFunction.UpdateFunction; }
            set
            {
                IterativeFunction.UpdateFunction = value;
                OnPropertyChanged("UpdateFunction");
                OnPropertyChanged("CanSubmit");
            }
        }

        private bool _wasSubmitted;
        public bool WasSubmitted
        {
            get { return _wasSubmitted; }
            set
            {
                _wasSubmitted = value;
                OnPropertyChanged("WasSubmitted");
                OnPropertyChanged("CanSubmit");
            }
        }

        public bool CanSubmit
        {
            get
            {
                return IsComplete && (! WasSubmitted);
            }
        }

        bool IsComplete
        {
            get
            {
                return IterativeFunction.Iterations.HasValue
                        && (IterativeFunction.InitialCondition != null)
                        && (!String.IsNullOrEmpty(IterativeFunction.Name))
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
                            o => CanSubmit
                        )
                    );
            }
        }
    }
}
