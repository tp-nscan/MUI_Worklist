using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using CommonUI;

namespace MefMuiApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // bootstrap MEF composition
            //var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            var catalog = new AggregateCatalog
                (
                    new AssemblyCatalog(Assembly.GetExecutingAssembly())
                    ,
                    new DirectoryCatalog(".")
                );

            var container = new CompositionContainer(catalog);

            // retrieve the MefContentLoader export and assign to global resources (so {DynamicResource MefContentLoader} can be resolved)
            var contentLoader = container.GetExport<MefContentLoader>().Value;
            this.Resources.Add("MefContentLoader", contentLoader);

            // same for MefLinkNavigator
            var navigator = container.GetExport<MefLinkNavigator>().Value;
            this.Resources.Add("MefLinkNavigator", navigator);
        }
    }
}
