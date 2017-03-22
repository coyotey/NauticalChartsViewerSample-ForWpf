using System;
using System.Windows;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class AreaDrawingModeMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string chartsOverlayName = "ChartsOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            Globals.CurrentBoundaryDisplayMode = (NauticalChartsBoundaryDisplayMode)Enum.Parse(typeof(NauticalChartsBoundaryDisplayMode), message.MenuItem.Action, true);
            if (map.Overlays.Contains(chartsOverlayName))
            {
                LayerOverlay chartsOverlay = map.Overlays[chartsOverlayName] as LayerOverlay;
                foreach (var item in chartsOverlay.Layers)
                {
                    NauticalChartsFeatureLayer maritimeFeatureLayer = item as NauticalChartsFeatureLayer;
                    maritimeFeatureLayer.BoundaryDisplayMode = Globals.CurrentBoundaryDisplayMode;
                }
                map.Refresh();
            }
        }

        public override string[] Actions
        {
            get { return new[] { "Plain", "Symbolized" }; }
        }
    }
}
