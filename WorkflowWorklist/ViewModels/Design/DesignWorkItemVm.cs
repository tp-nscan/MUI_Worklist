using System;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels.Design
{
    public class DesignWorkItemViewVm : WorkItemViewVmImpl
    {
        public DesignWorkItemViewVm()
            : base(workItem.Guid, "design Message", null)
        {
        }

        private static readonly IWorkItem workItem = IterativeWorkItem.Make
            (
                name: "design Message",
                guid: Guid.NewGuid(),
                initialConditon: "initial condition",
                updateOperation: s=>s,
                totalIterations: 5
            );
    }
}
