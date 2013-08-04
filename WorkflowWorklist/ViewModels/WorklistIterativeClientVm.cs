using System;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public class WorklistIterativeClientVm<T> : NotifyPropertyChanged where T : class
    {
        public WorklistIterativeClientVm
            (
                IWorklist worklist,
                IIterativeFunctionVm<T> iterativeFunctionVm,
                Func<IWorklist, Guid, IWorklistResultVm> worklistResultMaker 
            )
        {
            _iterativeFunctionVm = iterativeFunctionVm;
            IterativeFunctionVm.SubmitFunctionEvent.Subscribe(SubmitHandler);

            _worklistResultVm = worklistResultMaker(worklist, IterativeFunctionVm.Guid);
        }

        private readonly IIterativeFunctionVm<T> _iterativeFunctionVm;
        public IIterativeFunctionVm<T> IterativeFunctionVm
        {
            get { return _iterativeFunctionVm; }
        }

        private readonly IWorklistResultVm _worklistResultVm;
        public IWorklistResultVm WorklistResultVm
        {
            get { return _worklistResultVm; }
        }

        void SubmitHandler(IterativeFunction<T> fun)
        {
            WorklistResultVm.Worklist.PushIterative
            (
                name: fun.Name,
                guid: fun.Guid,
                initialCondidtion: fun.InitialCondition,
                iterativeOp: fun.UpdateFunction,
                iterations: fun.Iterations.HasValue ? fun.Iterations.Value : 0
            );

            WorklistResultVm.Worklist.Start();
        }
    }
}
