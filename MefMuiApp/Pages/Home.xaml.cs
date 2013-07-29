using CommonUI;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using MefMuiApp.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MefMuiApp.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    [ModernUiContent("/Home")]
    public partial class Home : UserControl, IContent, IPartImportsSatisfiedNotification
    {
        public Home()
        {
            InitializeComponent();
        }

        [Import]
        private HomeViewModel HomeVM { get; set; }

        public void OnImportsSatisfied()
        {
            this.DataContext = this.HomeVM;
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
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
