using WorkflowWorklist.ViewModels;

namespace MefMuiApp.ViewModels
{
    public class StringCatFunctionVm : IterativeFunctionVm<string>
    {

        public StringCatFunctionVm()
        {
            UpdateFunction = s => s + "_next";
        }
    }
}
