using System;
using System.Windows;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class ColorSchemaMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string chartsOverlayName = "ChartsOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            Globals.CurrentColorSchema = (NauticalChartsDefaultColorSchema)Enum.Parse(typeof(NauticalChartsDefaultColorSchema), message.MenuItem.Action, true);
            if (map.Overlays.Contains(chartsOverlayName))
            {
                LayerOverlay chartsOverlay = map.Overlays[chartsOverlayName] as LayerOverlay;
                foreach (var item in chartsOverlay.Layers)
                {
                    NauticalChartsFeatureLayer maritimeFeatureLayer = item as NauticalChartsFeatureLayer;
                    maritimeFeatureLayer.DefaultColorSchema = Globals.CurrentColorSchema;
                }
                map.Refresh();
            }
        }

        public override string[] Actions
        {
            get { return new[] { "daybright", "dayblackback", "daywhiteback", "dusk", "night" }; }
        }
    }
}
