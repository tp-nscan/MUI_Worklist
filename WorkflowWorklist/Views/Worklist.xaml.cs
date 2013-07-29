using System.ComponentModel.Composition;
using System.Windows.Controls;
using CommonUI;
using FirstFloor.ModernUI.Windows;
using WorkflowWorklist.ViewModels;
using FragmentNavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs;
using NavigatingCancelEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs;
using NavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs;

namespace MefMuiApp.Pages
{
    /// <summary>
    /// Interaction logic for Worklist.xaml
    /// </summary>
    [ModernUiContent("/Worklist")]
    public partial class Worklist : UserControl, IContent, IPartImportsSatisfiedNotification
    {
       const string Spank = "Worklist";

        public Worklist()
        {
            InitializeComponent();
            //Tabby.Links
        }


        [Import(Spank)]
        public WorklistVm WorkListVm { get; set; }

        public void OnImportsSatisfied()
        {
            this.DataContext = this.WorkListVm;
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

    }
}
