using System;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels.Design
{
    public class DesignWorkItemVm : WorkItemVmImpl
    {
        public DesignWorkItemVm()
            : base(workItem.Guid, "design name", WorkItemVmState.Scheduled, null)
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
