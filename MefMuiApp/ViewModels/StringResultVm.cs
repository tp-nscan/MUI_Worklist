using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowWorklist.ViewModels;

namespace MefMuiApp.ViewModels
{
    public class StringResultVm : WorklistResultVm<string>
    {
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
