using CommonUI;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using MefMuiApp.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MefMuiApp.Content
{
    /// <summary>
    /// Interaction logic for SettingsAppearance.xaml
    /// </summary>
    [ModernUiContent("/Settings/Appearance")]
    public partial class SettingsAppearance : UserControl, IContent, IPartImportsSatisfiedNotification
    {
        public SettingsAppearance()
        {
            InitializeComponent();
        }

        [Import]
        private SettingsAppearanceViewModel AppearanceVM { get; set; }

        public void OnImportsSatisfied()
        {
            this.DataContext = this.AppearanceVM;
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
