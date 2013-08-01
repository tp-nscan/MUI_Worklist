using System.Collections.Generic;
using WorkflowWorklist.Models;

namespace WorkflowWorklist.ViewModels.Design
{
    public class DesignWorklistVm : WorklistVm
    {
        public DesignWorklistVm()
        {
            var i = 0;
            foreach (var workItem in WorkItems)
            {
                var workItemVm = WorkItemViewVm.Schedule(workItem.Guid, "design name", Worklist);
                workItemVm.WorkItemStatus = (WorkItemStatus)((i++) % 6);
                WorkItemVMs.Add(workItemVm);
                i++;
            }
        }

        static IWorklist Worklist
        {
            get { var wklist = new Worklist();
                return wklist;
            }
        }

        static IEnumerable<IWorkItem> WorkItems
        {
            get
            {
                yield return IterativeWorkItem.Test(0);
                yield return IterativeWorkItem.Test(1);
                yield return IterativeWorkItem.Test(2);
                yield return IterativeWorkItem.Test(3);
                yield return IterativeWorkItem.Test(4);
                yield return IterativeWorkItem.Test(5);
                yield return IterativeWorkItem.Test(6);
                yield return IterativeWorkItem.Test(7);
            }
        }
    }
}
