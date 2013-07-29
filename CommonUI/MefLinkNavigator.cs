using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace CommonUI
{
    /// <summary>
    /// Extends the default link navigator by adding exported ICommands.
    /// </summary>
    [Export]
    public class MefLinkNavigator
        : DefaultLinkNavigator, IPartImportsSatisfiedNotification
    {
        [ImportMany]
        private Lazy<ICommand, ICommandMetadata>[] ImportedCommands { get; set; }

        public void OnImportsSatisfied()
        {
            // add the imported commands to the command dictionary
            foreach (var c in this.ImportedCommands) {
                var commandUri = new Uri(c.Metadata.CommandUri, UriKind.RelativeOrAbsolute);
                this.Commands.Add(commandUri, c.Value);
            }
        }
    }
}
