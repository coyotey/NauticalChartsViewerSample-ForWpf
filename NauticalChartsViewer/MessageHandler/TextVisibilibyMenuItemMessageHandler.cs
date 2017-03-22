using System.Windows;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class TextVisibilibyMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string chartsOverlayName = "ChartsOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            switch (message.MenuItem.Action)
            {
                case "contourlabel":
                    Globals.IsDepthContourTextVisible = message.MenuItem.IsChecked;
                    break;

                case "soundinglabel":
                    Globals.IsSoundingTextVisible = message.MenuItem.IsChecked;
                    break;

                case "lightdescription":
                    Globals.IsLightDescriptionVisible = message.MenuItem.IsChecked;
                    break;

                case "textlabel":
                    if (message.MenuItem.IsChecked)
                    {
                        Globals.SymbolTextDisplayMode = NauticalChartsSymbolTextDisplayMode.English;
                    }
                    break;

                case "nationallanguagelabel":

                    if (message.MenuItem.IsChecked)
                    {
                        Globals.SymbolTextDisplayMode = NauticalChartsSymbolTextDisplayMode.NationalLanguage;
                    }
                    break;
                case "notextlabel":

                    if (message.MenuItem.IsChecked)
                    {
                        Globals.SymbolTextDisplayMode = NauticalChartsSymbolTextDisplayMode.None;
                    }
                    break;
            }
            if (map.Overlays.Contains(chartsOverlayName))
            {
                LayerOverlay chartsOverlay = map.Overlays[chartsOverlayName] as LayerOverlay;

                foreach (var item in chartsOverlay.Layers)
                {
                    NauticalChartsFeatureLayer nauticalChartsFeatureLayer = item as NauticalChartsFeatureLayer;
                    nauticalChartsFeatureLayer.IsDepthContourTextVisible = Globals.IsDepthContourTextVisible;
                    nauticalChartsFeatureLayer.IsLightDescriptionVisible = Globals.IsLightDescriptionVisible;
                    nauticalChartsFeatureLayer.IsSoundingTextVisible = Globals.IsSoundingTextVisible;
                    nauticalChartsFeatureLayer.SymbolTextDisplayMode = Globals.SymbolTextDisplayMode;
                }

                map.Refresh();
            }
        }

        public override string[] Actions
        {
            get { return new[] { "contourlabel", "soundinglabel", "lightdescription", "textlabel", "notextlabel", "nationallanguagelabel" }; }
        }
    }
}