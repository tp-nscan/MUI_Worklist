using System;
using System.ComponentModel.Composition;
using System.Threading;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;
using WorkflowWorklist.ViewModels;

namespace MefMuiApp.ViewModels
{

    [Export]
    public class BarneyVm : NotifyPropertyChanged, IPartImportsSatisfiedNotification
    {
        [Import]
        private Worklist Worklist { get; set; }

        public StringResultVm StringResultVm { get; set; }

        public IIterativeFunctionVm<string> StringCatFunctionVm { get; set; }

        private string _result;

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public void OnImportsSatisfied()
        {
            StringCatFunctionVm = IterativeFunctionVm;
            StringCatFunctionVm.SubmitFunctionEvent.Subscribe(SubmitHandler);

            StringResultVm = new StringResultVm(Worklist, StringCatFunctionVm.Guid);

        }

        void SubmitHandler(IterativeFunction<string> fun)
        {
            Worklist.PushIterative
            (
                name: fun.Name,
                guid: fun.Guid,
                initialCondidtion: fun.InitialCondition,
                iterativeOp: fun.UpdateFunction,
                iterations:  fun.Iterations.HasValue ? fun.Iterations.Value : 0
            );

            Worklist.Start();
        }

        IIterativeFunctionVm<string> IterativeFunctionVm = new StringCatFunctionVm
        {
            InitialCondition = "bb",
            Iterations = 6,
            Name = "Barney",
            UpdateFunction = s =>
            {
                Thread.Sleep(1000);
                return s + "_next";
            }
        };

        WorklistIterativeClientVm<string> _worklistIterativeClientVm;
        public WorklistIterativeClientVm<string> WorklistIterativeClientVm
        {
            get { return _worklistIterativeClientVm; }
            set
            {
                _worklistIterativeClientVm = value;
                OnPropertyChanged("WorklistIterativeClientVm");
            }
        }
    }
}
