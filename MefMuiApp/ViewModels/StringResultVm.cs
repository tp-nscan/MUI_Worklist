using System;
using WorkflowWorklist.Models;
using WorkflowWorklist.ViewModels;

namespace MefMuiApp.ViewModels
{
    public class StringResultVm : WorkItemResultVm<string>
    {
        public StringResultVm(IWorklist worklist, Guid guid) : base(worklist, guid)
        {
        }

        protected override void ProcessResult(string result)
        {
            base.ProcessResult(result);
            OnPropertyChanged("StringResult");
        }

        public string StringResult
        {
            get { return _result; }
        }
    }
}
