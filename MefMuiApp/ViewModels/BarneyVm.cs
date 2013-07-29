using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Input;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace MefMuiApp.ViewModels
{
    [Export]
    public class BarneyVm : NotifyPropertyChanged
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
                            name: "name",
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

        private Guid _guid = Guid.Empty;
    }
}
