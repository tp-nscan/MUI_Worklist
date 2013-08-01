using System;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels.Design
{
    public class DesignWorkItemViewViewVm : WorkItemViewViewVmImpl
    {
        public DesignWorkItemViewViewVm()
            : base(workItem.Guid, "design name", null)
        {
        }

        private static readonly IWorkItem workItem = IterativeWorkItem.Make
            (
                name: "design name",
                guid: Guid.NewGuid(),
                initialConditon: "initial condition",
                updateOperation: s=>s,
                totalIterations: 5
            );
    }
}
