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
        private string _name = "Barney";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Import]
        private Worklist Worklist { get; set; }

        private ICommand _add;
        public ICommand Add
        {
            get
            {
                return _add ?? (_add = new RelayCommand
                (
                    o =>
                    {
                        Worklist.PushIterative
                        (
                            name: "from barney",
                            guid: (_guid = Guid.NewGuid()),
                            initialCondidtion: "hi",
                            iterativeOp: s =>
                            {
                                Thread.Sleep(1000);
                                return s + "_next";
                            },
                            iterations: 6
                        );

                        Worklist.Start();            
                    },
                    o => true
                ));
            }
        }

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
            Worklist.OnWorklistEvent.Subscribe(HandleWorklistNotice);
        }

        void HandleWorklistNotice(WorklistEventArgs e)
        {
            if (e.WorkItemInfo.Guid == _guid)
            {
                Result = (string) e.WorkItemInfo.Result;
            }
        }
    }
}
