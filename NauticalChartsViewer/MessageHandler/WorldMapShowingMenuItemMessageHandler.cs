using System.Windows;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    internal class WorldMapShowingMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string worldMapOverlayName = "WorldMapOverlay";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            WorldMapKitWmsWpfOverlay worldMapKitOverlay;
            if (map.Overlays.Contains(worldMapOverlayName))
            {
                worldMapKitOverlay = map.Overlays[worldMapOverlayName] as WorldMapKitWmsWpfOverlay;
            }
            else
            {
                worldMapKitOverlay = new WorldMapKitWmsWpfOverlay();
                map.Overlays.Insert(0, worldMapKitOverlay);
            }

            worldMapKitOverlay.IsVisible = true;
            switch (message.MenuItem.Action.ToLowerInvariant())
            {
                case "streetmap":
                    worldMapKitOverlay.MapType = WorldMapKitMapType.Road;
                    break;

                case "aerial":
                    worldMapKitOverlay.MapType = WorldMapKitMapType.Aerial;
                    break;

                case "aerialwithlabels":
                    worldMapKitOverlay.MapType = WorldMapKitMapType.AerialWithLabels;
                    break;

                case "none":
                    worldMapKitOverlay.IsVisible = false;
                    break;
            }

            map.Refresh(worldMapKitOverlay);
        }

        public override string[] Actions
        {
            get { return new[] { "streetmap", "aerial", "aerialwithlabels", "none" }; }
        }
    }
}