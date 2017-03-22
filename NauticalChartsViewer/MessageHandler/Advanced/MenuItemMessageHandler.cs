using System.ComponentModel.Composition;
using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    [InheritedExport]
    internal abstract class MenuItemMessageHandler
    {
        public abstract void Handle(Window owner, WpfMap map, MenuItemMessage message);

        public abstract string[] Actions { get; }
    }
}
