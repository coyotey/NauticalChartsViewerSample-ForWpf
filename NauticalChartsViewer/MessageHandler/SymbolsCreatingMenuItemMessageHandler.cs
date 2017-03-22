using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class SymbolsCreatingMenuItemMessageHandler : MenuItemMessageHandler
    {
        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            var window = new SymbolsCreatingWindow();
            window.Owner = owner;
            window.ShowDialog();
        }

        public override string[] Actions
        {
            get { return new[] { "createsymbols" }; }
        }
    }
}
