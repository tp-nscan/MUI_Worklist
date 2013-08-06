using System.Windows;
using WorkflowWorklist.ViewModels;

namespace WorkflowWorklist.Views
{
    public partial class WorkItemControl
    {
        public WorkItemControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty WorkItemControllerProperty =
             DependencyProperty.Register("WorkItemController", typeof(IWorkItemControllerVm),
             typeof(WorkItemControl), new FrameworkPropertyMetadata(OnCurrentTimePropertyChanged));

        private static void OnCurrentTimePropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            
        }

        public IWorkItemControllerVm WorkItemController
        {
            get { return (IWorkItemControllerVm)GetValue(WorkItemControllerProperty); }
            set { SetValue(WorkItemControllerProperty, value); }
        }


    }
}
