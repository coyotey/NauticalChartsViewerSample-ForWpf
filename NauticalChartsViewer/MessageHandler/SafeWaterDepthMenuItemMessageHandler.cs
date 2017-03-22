using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class SafeWaterDepthMenuItemMessageHandler : MenuItemMessageHandler
    {
        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            var window = new SafeWaterDepthSettingsWindow();
            window.Owner = owner;
            window.ShowDialog();
        }

        public override string[] Actions
        {
            get { return new[] { "safewaterdepth" }; }
        }
    }
}
