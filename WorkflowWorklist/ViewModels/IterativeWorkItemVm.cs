using System;
using FirstFloor.ModernUI.Presentation;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels
{
    public class IterativeWorkItemVm<T> : NotifyPropertyChanged, IWorkItemControllerVm where T : class
    {
        public IterativeWorkItemVm
            (
                IWorklist worklist,
                IIterativeFunctionVm<T> iterativeFunctionVm,
                Func<IWorklist, Guid, IWorkItemMonitorVm> worklistMonitorMaker,
                Func<IWorklist, Guid, IWorkItemResultVm> worklistResultMaker 
            )
        {
            _iterativeFunctionVm = iterativeFunctionVm;
            IterativeFunctionVm.SubmitFunctionEvent.Subscribe(SubmitHandler);
            _workItemMonitorVm = worklistMonitorMaker(worklist, IterativeFunctionVm.Guid);
            _workItemResultVm = worklistResultMaker(worklist, IterativeFunctionVm.Guid);
        }

        private readonly IIterativeFunctionVm<T> _iterativeFunctionVm;
        public IIterativeFunctionVm<T> IterativeFunctionVm
        {
            get { return _iterativeFunctionVm; }
        }

        private readonly IWorkItemMonitorVm _workItemMonitorVm;

        public ISubmitFunctionVm SubmitFunctionVm
        {
            get { return IterativeFunctionVm; }
        }

        public IWorkItemMonitorVm WorkItemMonitorVm
        {
            get { return _workItemMonitorVm; }
        }

        private readonly IWorkItemResultVm _workItemResultVm;
        public IWorkItemResultVm WorkItemResultVm
        {
            get { return _workItemResultVm; }
        }

        void SubmitHandler(IterativeFunction<T> fun)
        {
            WorkItemResultVm.Worklist.PushIterative
            (
                name: fun.Name,
                guid: fun.Guid,
                initialCondidtion: fun.InitialCondition,
                iterativeOp: fun.UpdateFunction,
                iterations: fun.Iterations.HasValue ? fun.Iterations.Value : 0
            );

            WorkItemResultVm.Worklist.Start();
        }

    }
}
