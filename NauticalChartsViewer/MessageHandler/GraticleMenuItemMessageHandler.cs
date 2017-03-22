using System.Windows;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class GraticleMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string GraticuleOverlayName = "GraticuleOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            LayerOverlay adornmentOverlay;
            if (!map.Overlays.Contains(GraticuleOverlayName))
            {
                var graticuleLayer = new GraticuleFeatureLayer()
                {
                    GraticuleLineStyle = new LineStyle(new GeoPen(GeoColor.StandardColors.LightGray))
                };
                adornmentOverlay = new LayerOverlay();
                adornmentOverlay.Layers.Add(graticuleLayer);
                map.Overlays.Add(GraticuleOverlayName, adornmentOverlay);
            }

            adornmentOverlay = map.Overlays[GraticuleOverlayName] as LayerOverlay;
            adornmentOverlay.IsVisible = message.MenuItem.IsChecked;

            map.Refresh();
        }

        public override string[] Actions
        {
            get { return new[] { "graticule" }; }
        }
    }
}
