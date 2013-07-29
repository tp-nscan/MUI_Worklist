using System;
using System.ComponentModel.Composition;
using FirstFloor.ModernUI.Windows;

namespace CommonUI
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModernUiContentAttribute
        : ExportAttribute
    {
        public ModernUiContentAttribute(string contentUri)
            : base(typeof(IContent))
        {
            this.ContentUri = contentUri;
        }
        public string ContentUri { get; private set; }
    }
}
