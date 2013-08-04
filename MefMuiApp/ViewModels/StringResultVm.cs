using System;
using WorkflowWorklist.Models;
using WorkflowWorklist.ViewModels;

namespace MefMuiApp.ViewModels
{
    public class StringResultVm : WorklistResultVm<string>
    {
        public StringResultVm(IWorklist worklist, Guid guid) : base(worklist, guid)
        {
        }

        protected override void ProcessResult(string result)
        {
            Result = result;
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
    }
}
