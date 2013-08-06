
using System.Windows;
using WorkflowWorklist.ViewModels;

namespace WorkflowWorklist.Views
{
    /// <summary>
    /// Interaction logic for WorkItemInfo.xaml
    /// </summary>
    public partial class WorkItemMonitorControl
    {
        public WorkItemMonitorControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty WorkItemMonitorProperty =
         DependencyProperty.Register("WorkItemMonitor", typeof(IWorkItemMonitorVm),
         typeof(WorkItemMonitorControl));

        public IWorkItemMonitorVm WorkItemMonitor
        {
            get { return (IWorkItemMonitorVm)GetValue(WorkItemMonitorProperty); }
            set { SetValue(WorkItemMonitorProperty, value); }
        }
    }
}
