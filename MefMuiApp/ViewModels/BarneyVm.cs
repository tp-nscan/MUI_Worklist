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

        public void OnImportsSatisfied()
        {
            WorklistIterativeClientVm = new WorklistIterativeClientVm<string>
                (
                    worklist: Worklist,
                    iterativeFunctionVm: iterativeFunctionVm,
                    worklistResultMaker:  (w, g) => new StringResultVm(w, g)
                );

        }

        static readonly IIterativeFunctionVm<string> iterativeFunctionVm = new StringCatFunctionVm
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
