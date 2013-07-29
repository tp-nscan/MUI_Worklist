using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace CommonUI
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class CommandAttribute
        : ExportAttribute
    {
        public CommandAttribute(string commandUri)
            : base(typeof(ICommand))
        {
            this.CommandUri = commandUri;
        }
        public string CommandUri { get; private set; }
    }
}
