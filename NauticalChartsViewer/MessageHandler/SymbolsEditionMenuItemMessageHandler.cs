using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class SymbolsEditionMenuItemMessageHandler : MenuItemMessageHandler
    {
        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            SymbolsEditionWindow window = new SymbolsEditionWindow();
            window.Owner = owner;
            window.ShowDialog();
        }

        public override string[] Actions
        {
            get { return new[] { "editsymbols" }; }
        }
    }
}
