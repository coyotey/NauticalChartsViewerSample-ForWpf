using System;
using System.Windows;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class PerformanceModeMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string chartsOverlayName = "ChartsOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            Globals.CurrentDrawingMode = (NauticalChartsDrawingMode)Enum.Parse(typeof(NauticalChartsDrawingMode), message.MenuItem.Action, true);
            if (map.Overlays.Contains(chartsOverlayName))
            {
                LayerOverlay chartsOverlay = map.Overlays[chartsOverlayName] as LayerOverlay;
                foreach (var item in chartsOverlay.Layers)
                {
                    NauticalChartsFeatureLayer maritimeFeatureLayer = item as NauticalChartsFeatureLayer;
                    maritimeFeatureLayer.DrawingMode = Globals.CurrentDrawingMode;
                }
                map.Refresh();
            }
        }

        public override string[] Actions
        {
            get { return new[] { "optimized", "hightspeed", "hightquality" }; }
        }
    }
}
