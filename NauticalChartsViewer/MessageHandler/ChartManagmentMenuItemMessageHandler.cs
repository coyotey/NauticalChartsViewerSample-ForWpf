using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Wpf;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{
    internal class ChartManagmentMenuItemMessageHandler : MenuItemMessageHandler
    {
        private const string chartsOverlayName = "ChartsOverlay";
        private const string boundingBoxPreviewOverlayName = "BoundingBoxPreview";

        public override void Handle(Window owner, WpfMap map, MenuItemMessage message)
        {
            switch (message.MenuItem.Action.ToLowerInvariant())
            {
                case "loadcharts":
                    {
                        var window = new ChartsManagmentWindow();
                        window.Owner = owner;
                        window.ShowDialog();
                    }
                    break;

                case "unloadcharts":
                    {
                        if (map.Overlays.Contains(chartsOverlayName))
                        {
                            if (MessageBox.Show("Do you want to unload all the charts?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if (map.Overlays.Contains(chartsOverlayName))
                                {
                                    GeoCollection<Layer> layers = (map.Overlays[chartsOverlayName] as LayerOverlay).Layers;
                                    foreach (Layer layer in layers)
                                    {
                                        layer.Close();
                                    }
                                    layers.Clear();

                                    ChartMessage chartMessage = new ChartMessage(ChartsManagmentViewModel.Instance.Charts);
                                    Messenger.Default.Send<ChartMessage>(chartMessage, "UnloadCharts");
                                    ChartsManagmentViewModel.Instance.Charts.Clear();
                                    ChartsManagmentViewModel.Instance.SelectedItem = null;
                                    ChartsManagmentViewModel.Instance.SelectedItems.Clear();
                                }

                                if (map.Overlays.Contains(boundingBoxPreviewOverlayName))
                                {
                                    ((InMemoryFeatureLayer)((LayerOverlay)map.Overlays[boundingBoxPreviewOverlayName]).Layers[0]).InternalFeatures.Clear();
                                }
                                map.Refresh();
                            }
                        }
                    }
                    break;
            }
        }

        public override string[] Actions
        {
            get { return new[] { "loadcharts", "unloadcharts" }; }
        }
    }
}