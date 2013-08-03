using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace MefMuiApp.ViewModels
{

    [Export]
    public class BarneyVm : NotifyPropertyChanged, IPartImportsSatisfiedNotification
    {
        [Import]
        private Worklist Worklist { get; set; }

        public StringResultVm StringResultVm { get; set; }

        public StringCatFunctionVm StringCatFunctionVm { get; set; }

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

        private Guid _guid = Guid.Empty;
        public void OnImportsSatisfied()
        {
            StringResultVm = new StringResultVm();
            StringResultVm.SetWorklist(Worklist);

            StringCatFunctionVm = new StringCatFunctionVm();
            StringCatFunctionVm.InitialCondition = "bb";
            StringCatFunctionVm.Iterations = 6;
            StringCatFunctionVm.Name = "Barney";
            StringCatFunctionVm.UpdateFunction = s =>
            {
                Thread.Sleep(1000);
                return s + "_next";
            };

            StringCatFunctionVm.SubmitFunctionEvent.Subscribe(SubmitHandler);
        }

        void SubmitHandler(IterativeFunction<string> fun)
        {
            Worklist.PushIterative
            (
                name: fun.Name,
                guid: (_guid = fun.Guid),
                initialCondidtion: fun.InitialCondition,
                iterativeOp: fun.UpdateFunction,
                iterations:  fun.Iterations.HasValue ? fun.Iterations.Value : 0
            );

            Worklist.Start();
        }

    }
}
