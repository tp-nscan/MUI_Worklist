using System.ComponentModel.Composition;
using System.Windows.Controls;
using CommonUI;
using FirstFloor.ModernUI.Windows;
using MefMuiApp.ViewModels;
using FragmentNavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs;
using NavigatingCancelEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs;
using NavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs;

namespace MefMuiApp.Pages
{
    /// <summary>
    /// Interaction logic for Barney.xaml
    /// </summary>    
    [ModernUiContent("/Barney")]
    public partial class Barney : UserControl, IContent, IPartImportsSatisfiedNotification
    {
        public Barney()
        {
            InitializeComponent();
        }

        [Import]
        private BarneyVm BarneyViewModel { get; set; }

        public void OnImportsSatisfied()
        {
            this.DataContext = this.BarneyViewModel;
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
