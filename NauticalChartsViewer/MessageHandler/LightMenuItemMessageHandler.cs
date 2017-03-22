using System.Windows;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class LightsMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string chartsOverlayName = "ChartsOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            Globals.IsFullLightLineVisible = message.MenuItem.IsChecked;
            if (map.Overlays.Contains(chartsOverlayName))
            {
                LayerOverlay chartsOverlay = map.Overlays[chartsOverlayName] as LayerOverlay;
                foreach (var item in chartsOverlay.Layers)
                {
                    NauticalChartsFeatureLayer maritimeFeatureLayer = item as NauticalChartsFeatureLayer;
                    maritimeFeatureLayer.IsFullLightLineVisible = Globals.IsFullLightLineVisible;
                }
                map.Refresh();
            }
        }

        public override string[] Actions
        {
            get { return new[] { "lights" }; }
        }
    }
}