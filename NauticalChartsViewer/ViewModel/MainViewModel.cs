using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.Wpf;

namespace NauticalChartsViewer
{
    public class MainViewModel : ViewModelBase
    {
        private const string boundingBoxPreviewOverlayName = "BoundingBoxPreview";
        private const string chartsOverlayName = "ChartsOverlay";
        private const string GraticuleOverlayName = "GraticuleOverlay";
        private const string highlightOverlayName = "highlight";
        private const string WorldMapOverlayName = "WorldMapOverlay";

        private InMemoryFeatureLayer boundingBoxPreviewLayer;
        private ChartSelectedItem chartSelectedItem = new ChartSelectedItem(string.Empty, null);
        private double overlayOpacity = 1;
        private bool isIdentify = false;
        private bool canHandleExecute = true;
        private bool isOnLoading;
        private bool showOpacityPanel;
        private WpfMap map;
        private Collection<object> menuItems;

        [ImportMany(typeof(MenuItemMessageHandler), AllowRecomposition = false)]
        private Collection<MenuItemMessageHandler> messageHandlers;

        private ICommand clearSelectionCommand;
        private ICommand opacityPanelCloseCommand;
        private ICommand toolBarCommand;

        private Collection<BaseMenuItem> areaDrawingModes;
        private Collection<BaseMenuItem> baseMaps;
        private Collection<BaseMenuItem> colorSchemas;
        private Collection<BaseMenuItem> displayCategorys;
        private Collection<BaseMenuItem> pointDrawingModes;
        private Collection<BaseMenuItem> symbolLabels;
        private BaseMenuItem selectedAreaDrawingMode;
        private BaseMenuItem selectedBaseMap;
        private BaseMenuItem selectedColorSchema;
        private BaseMenuItem selectedDisplayCategory;
        private FeatureInfo selectedFeatureInfo;
        private BaseMenuItem selectedPointDrawingMode;
        private BaseMenuItem selectedSymbolLabel;
        private BaseMenuItem showContourText;
        private BaseMenuItem showingGradicule;
        private BaseMenuItem showLightDescriptions;
        private BaseMenuItem showLights;
        private BaseMenuItem showSoundingText;
      
     
        public MainViewModel(WpfMap map)
        { 
            this.map = map;
            map.MapClick += WpfMap_MapClick;  
            menuItems = new Collection<object>(MenuItemHelper.GetMenus());
            
            LoadMessageHandlers();
            SetToolbarMenuItems();

            Messenger.Default.Register<ChartMessage>(this, (m) => ChartSelectedItem = new ChartSelectedItem(string.Empty, null));
            Messenger.Default.Register<MenuItemMessage>(this, "ShowOpacityPanel", (m) => ShowOpacityPanel = true);
            Messenger.Default.Register<MenuItemMessage>(this, HandleMenuItemMessage);
            Messenger.Default.Register<ToolBarMessage>(this, HandleToolBarMessage);
            Messenger.Default.Register<ChartMessage>(this, "LoadCharts", HandleLoadChartMessage);
            Messenger.Default.Register<ChartMessage>(this, "UnloadCharts", HandleUnloadChartMessage);
            Messenger.Default.Register<ChartSelectedItemMessage>(this, HandleChartSelectedItemMessage);
            Messenger.Default.Register<SafeWaterDepthSettingMessage>(this, HandleSafeWaterDepthMessage);

            map.CurrentExtent = new RectangleShape(-130, 40, -30, 5);
            map.Overlays.Add(WorldMapOverlayName, new WorldMapKitWmsWpfOverlay());

            InitBoundingBoxPreviewOverlay(map);
        }

        public Collection<BaseMenuItem> AreaDrawingModes
        {
            get { return areaDrawingModes; }
        }

        public Collection<BaseMenuItem> BaseMaps
        {
            get { return baseMaps; }
        }

        public ChartSelectedItem ChartSelectedItem
        {
            get { return chartSelectedItem; }
            set
            {
                if (chartSelectedItem != value)
                {
                    chartSelectedItem = value;
                    RaisePropertyChanged(() => ChartSelectedItem);
                }
            }
        }

        public ICommand ClearSelectionCommand
        {
            get { return clearSelectionCommand ?? (clearSelectionCommand = new RelayCommand(HandleClearSelectionCommand)); }
        }

        public Collection<BaseMenuItem> ColorSchemas
        {
            get { return colorSchemas; }
        }

        public Collection<BaseMenuItem> DisplayCategorys
        {
            get { return displayCategorys; }
        }

        public bool IsIdentify
        {
            get { return isIdentify; }
            set
            {
                map.Cursor = (value ? Cursors.Cross : null);
                isIdentify = value;
            }
        }

        public bool IsOnLoading
        {
            get { return isOnLoading; }
            set
            {
                isOnLoading = value;
                RaisePropertyChanged(() => IsOnLoading);
            }
        }

        public Collection<object> MenuItems
        {
            get { return menuItems; }
        }

        public ICommand OpacityPanelCloseCommand
        {
            get { return opacityPanelCloseCommand ?? (opacityPanelCloseCommand = new RelayCommand(() => ShowOpacityPanel = false)); }
        }

        public double OverlayOpacity
        {
            get { return overlayOpacity; }
            set
            {
                if (Math.Abs(overlayOpacity - value) > double.Epsilon)
                {
                    overlayOpacity = value;
                    ApplyOverlayOpacity();
                    RaisePropertyChanged(() => OverlayOpacity);
                }
            }
        }

        public Collection<BaseMenuItem> PointDrawingModes
        {
            get { return pointDrawingModes; }
        }

        public BaseMenuItem SelectedAreaDrawingMode
        {
            get { return selectedAreaDrawingMode; }
            set
            {
                if (selectedAreaDrawingMode != value)
                {
                    selectedAreaDrawingMode = value;
                    if (canHandleExecute)
                    {
                        value.SelectedCommand.Execute(null);
                    }
                    RaisePropertyChanged(() => SelectedAreaDrawingMode);
                }
            }
        }

        public BaseMenuItem SelectedBaseMap
        {
            get { return selectedBaseMap; }
            set
            {
                selectedBaseMap = value;
                if (canHandleExecute)
                {
                    value.SelectedCommand.Execute(null);
                }
                RaisePropertyChanged(() => SelectedBaseMap);
            }
        }

        public BaseMenuItem SelectedColorSchema
        {
            get { return selectedColorSchema; }
            set
            {
                if (selectedColorSchema != value)
                {
                    selectedColorSchema = value;
                    if (canHandleExecute)
                    {
                        value.SelectedCommand.Execute(null);
                    }
                    RaisePropertyChanged(() => SelectedColorSchema);
                }
            }
        }

        public BaseMenuItem SelectedDisplayCategory
        {
            get { return selectedDisplayCategory; }
            set
            {
                if (selectedDisplayCategory != value)
                {
                    selectedDisplayCategory = value;
                    if (canHandleExecute)
                    {
                        value.SelectedCommand.Execute(null);
                    }
                    RaisePropertyChanged(() => SelectedDisplayCategory);
                }
            }
        }

        public FeatureInfo SelectedFeatureInfo
        {
            get { return selectedFeatureInfo; }
            set
            {
                if (selectedFeatureInfo != value)
                {
                    selectedFeatureInfo = value;
                    HandleFeatureSelectedChanged(value);
                    RaisePropertyChanged(() => SelectedFeatureInfo);
                }
            }
        }

        public BaseMenuItem SelectedPointDrawingMode
        {
            get { return selectedPointDrawingMode; }
            set
            {
                if (selectedPointDrawingMode != value)
                {
                    selectedPointDrawingMode = value;
                    if (canHandleExecute)
                    {
                        value.SelectedCommand.Execute(null);
                    }
                    RaisePropertyChanged(() => SelectedPointDrawingMode);
                }
            }
        }

        public BaseMenuItem SelectedSymbolLabel
        {
            get { return selectedSymbolLabel; }
            set
            {
                if (selectedSymbolLabel != value)
                {
                    selectedSymbolLabel = value;
                    if (canHandleExecute)
                    {
                        value.SelectedCommand.Execute(null);
                    }
                    RaisePropertyChanged(() => SelectedSymbolLabel);
                }
            }
        }

        public BaseMenuItem ShowContourText
        {
            get { return showContourText; }
            set { showContourText = value; }
        }

        public BaseMenuItem ShowingGradicule
        {
            get { return showingGradicule; }
            set
            {
                if (showingGradicule != value)
                {
                    showingGradicule = value;
                    RaisePropertyChanged(() => ShowingGradicule);
                }
            }
        }

        public BaseMenuItem ShowLightDescriptions
        {
            get { return showLightDescriptions; }
            set { showLightDescriptions = value; }
        }

        public BaseMenuItem ShowLights
        {
            get { return showLights; }
            set { showLights = value; }
        }

        public bool ShowOpacityPanel
        {
            get { return showOpacityPanel; }
            set
            {
                if (showOpacityPanel != value)
                {
                    showOpacityPanel = value;
                    RaisePropertyChanged(() => ShowOpacityPanel);
                }
            }
        }

        public BaseMenuItem ShowSoundingText
        {
            get { return showSoundingText; }
            set { showSoundingText = value; }
        }

        public Collection<BaseMenuItem> SymbolLabels
        {
            get { return symbolLabels; }
        }

        public ICommand ToolBarCommand
        {
            get { return toolBarCommand ?? (toolBarCommand = new RelayCommand<string>(HandleToolBarCommand)); }
        }

        public override void Cleanup()
        {
            map.MapClick -= WpfMap_MapClick;
            Messenger.Default.Unregister(this);
            base.Cleanup();
        }

        internal void ApplyOverlayOpacity()
        {
            if (map.Overlays.Contains(chartsOverlayName))
            {
                LayerOverlay overlay = ((LayerOverlay)map.Overlays[chartsOverlayName]);
                overlay.OverlayCanvas.Opacity = OverlayOpacity;
                map.Refresh(overlay);
            }
        }

        private static BaseMenuItem GetMenuByAction(string action, Collection<object> sourceMenus)
        {
            if (sourceMenus != null)
            {
                foreach (object item in sourceMenus)
                {
                    if (item is CompositeMenuItem)
                    {
                        CompositeMenuItem compositeMenuItem = ((CompositeMenuItem)item);
                        BaseMenuItem menu = GetMenuByAction(action, compositeMenuItem.Children);
                        if (menu != null)
                        {
                            return menu;
                        }
                    }
                    else if (item is SingleMenuItem)
                    {
                        SingleMenuItem singleMenuItem = ((SingleMenuItem)item);
                        if (string.Compare(singleMenuItem.Action, action, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            return singleMenuItem;
                        }
                    }
                }
            }
            return null;
        }

        private static Collection<BaseMenuItem> GetMenusByGroupName(string groupName, Collection<object> sourceMenus)
        {
            var menus = new Collection<BaseMenuItem>();
            if (sourceMenus != null)
            {
                foreach (object item in sourceMenus)
                {
                    if (item is CompositeMenuItem)
                    {
                        CompositeMenuItem compositeMenuItem = ((CompositeMenuItem)item);
                        Collection<BaseMenuItem> recursions = GetMenusByGroupName(groupName, compositeMenuItem.Children);
                        foreach (BaseMenuItem recursionItem in recursions)
                        {
                            menus.Add(recursionItem);
                        }
                    }
                    else if (item is SingleMenuItem)
                    {
                        SingleMenuItem singleMenuItem = ((SingleMenuItem)item);
                        if (string.Compare(singleMenuItem.GroupName, groupName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            menus.Add((BaseMenuItem)item);
                        }
                    }
                }
            }
            return menus;
        }

        private LayerOverlay CreateHighlightLayerOverlay(Feature feature)
        {
            LayerOverlay highlightOverlay = new LayerOverlay();
            InMemoryFeatureLayer inMemoryFeatureLayer = new InMemoryFeatureLayer();
            //GeoHatchBrush fillBrush = new GeoHatchBrush(GeoHatchStyle.Cross, GeoColor.FromArgb(100, 0, 255, 0));
            inMemoryFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateHatchStyle(GeoHatchStyle.BackwardDiagonal, GeoColor.FromArgb(150, 254, 255, 39), GeoColor.StandardColors.Transparent);
            inMemoryFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.FromArgb(50, GeoColor.StandardColors.Green), 3, false);
            inMemoryFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            inMemoryFeatureLayer.InternalFeatures.Add(feature);

            highlightOverlay.Layers.Add(inMemoryFeatureLayer);
            return highlightOverlay;
        }

        private void HandleChartSelectedItemMessage(ChartSelectedItemMessage message)
        {
            throw new System.NotImplementedException();
        }

        private void HandleClearSelectionCommand()
        {
            ChartSelectedItem = new ChartSelectedItem(chartSelectedItem.FullName, new List<FeatureInfo>());
        }

        private void HandleFeatureSelectedChanged(object item)
        {
            FeatureInfo featureInfo = item as FeatureInfo;

            if (!map.Overlays.Contains(chartsOverlayName))
            {
                return;
            }
            if (map.Overlays.Contains(highlightOverlayName))
            {
                map.Overlays.Remove(highlightOverlayName);
            }

            if (featureInfo != null)
            {
                LayerOverlay overlay = map.Overlays[chartsOverlayName] as LayerOverlay;
                NauticalChartsFeatureLayer layer = overlay.Layers[featureInfo.LayerName] as NauticalChartsFeatureLayer;
                layer.Open();
                Feature feature = layer.QueryTools.GetFeatureById(featureInfo.Id, ReturningColumnsType.AllColumns);
                layer.Close();

                if (feature != null)
                {
                    LayerOverlay highlightOverlay = CreateHighlightLayerOverlay(feature);
                    map.Overlays.Add(highlightOverlayName, highlightOverlay);
                    map.CurrentExtent = feature.GetBoundingBox();
                }
            }

            map.Refresh();
        }

        private void HandleLoadChartMessage(ChartMessage message)
        {
            LayerOverlay overlay = null;
            if (message.Charts != null)
            {
                if (map.Overlays.Contains(chartsOverlayName))
                {
                    overlay = ((LayerOverlay)map.Overlays[chartsOverlayName]);
                }
                else
                {
                    overlay = new LayerOverlay()
                    {
                        TileType = TileType.SingleTile,
                    };
                    map.Overlays.Insert(1, chartsOverlayName, overlay);
                }

                overlay.Layers.Clear();
                ChartSelectedItem = new ChartSelectedItem(string.Empty, null);
                IEnumerable<ChartItem> charts = message.Charts;
                RectangleShape boundingBox = null;
                foreach (ChartItem item in charts)
                {
                    if (!File.Exists(item.IndexFileName))
                    {
                        NauticalChartsFeatureSource.BuildIndexFile(item.FileName, BuildIndexMode.DoNotRebuild);
                    }
                    NauticalChartsFeatureLayer layer = new NauticalChartsFeatureLayer(item.FileName);

                    layer.DrawingFeatures += hydrographyLayer_DrawingFeatures;

                    layer.IsDepthContourTextVisible = Globals.IsDepthContourTextVisible;
                    layer.IsLightDescriptionVisible = Globals.IsLightDescriptionVisible;
                    layer.IsSoundingTextVisible = Globals.IsSoundingTextVisible;
                    layer.SymbolTextDisplayMode = Globals.SymbolTextDisplayMode;
                    layer.DisplayCategory = Globals.DisplayMode;
                    layer.DefaultColorSchema = Globals.CurrentColorSchema;
                    layer.SymbolDisplayMode = Globals.CurrentSymbolDisplayMode;
                    layer.BoundaryDisplayMode = Globals.CurrentBoundaryDisplayMode;

                    layer.SafetyDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(Globals.SafetyDepth, Globals.CurrentDepthUnit);
                    layer.ShallowDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(Globals.ShallowDepth, Globals.CurrentDepthUnit);
                    layer.DeepDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(Globals.DeepDepth, Globals.CurrentDepthUnit);
                    layer.SafetyContourDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(Globals.SafetyContour, Globals.CurrentDepthUnit);

                    layer.DrawingMode = Globals.CurrentDrawingMode;
                    layer.IsFullLightLineVisible = Globals.IsFullLightLineVisible;
                    layer.IsMetaObjectsVisible = Globals.IsMetaObjectsVisible;
                    layer.Name = item.FileName;
                    layer.Open();
                    if (boundingBox == null)
                    {
                        boundingBox = layer.GetBoundingBox();
                    }
                    else
                    {
                        boundingBox.ExpandToInclude(layer.GetBoundingBox());
                    }

                    boundingBoxPreviewLayer.InternalFeatures.Add(layer.GetHashCode().ToString(), new Feature(layer.GetBoundingBox()));

                    layer.Close();
                    overlay.Layers.Add(item.FileName, layer);
                }
                if (boundingBox != null)
                {
                    map.CurrentExtent = boundingBox;
                }

                //SetupAnimationForOverlay(overlay);

                ApplyOverlayOpacity();

                map.Refresh();
            }
        }

        private void HandleMenuItemMessage(MenuItemMessage message)
        {
            if (message != null && message.MenuItem != null && !string.IsNullOrEmpty(message.MenuItem.Action))
            {
                foreach (var messageHandler in messageHandlers)
                {
                    if (messageHandler.Actions.Contains(message.MenuItem.Action))
                    {
                        messageHandler.Handle(Application.Current.MainWindow, map, message);
                    }
                }
            }
        }

        private void HandleSafeWaterDepthMessage(SafeWaterDepthSettingMessage message)
        {
            if (!map.Overlays.Contains(chartsOverlayName))
            {
                return;
            }

            LayerOverlay overlay = map.Overlays[chartsOverlayName] as LayerOverlay;
            var layers = overlay.Layers.OfType<NauticalChartsFeatureLayer>();
            foreach (NauticalChartsFeatureLayer layer in layers)
            {
                layer.SafetyDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(message.SafetyWaterDepth, message.DepthUnit);
                layer.ShallowDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(message.ShallowWaterDepth, message.DepthUnit);
                layer.DeepDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(message.DeepWaterDepth, message.DepthUnit);
                layer.SafetyContourDepthInMeter = NauticalChartsFeatureLayer.ConvertDistanceToMeters(message.SafetyContourDepth, message.DepthUnit);
            }
            Globals.SafetyDepth = message.SafetyWaterDepth;
            Globals.DeepDepth = message.DeepWaterDepth;
            Globals.ShallowDepth = message.ShallowWaterDepth;
            Globals.SafetyContour = message.SafetyContourDepth;
            Globals.CurrentDepthUnit = message.DepthUnit;
            map.Refresh();
        }

        private void HandleToolBarCommand(string action)
        {
            var message = new ToolBarMessage(action);
            Messenger.Default.Send<ToolBarMessage>(message);
        }

        private void HandleToolBarMessage(ToolBarMessage message)
        {
            if (message != null && !string.IsNullOrEmpty(message.Action))
            {
                switch (message.Action.ToLower())
                {
                    case "loadcharts":
                        {
                            var window = new ChartsManagmentWindow();
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();
                        }
                        break;
                }
            }
        }

        private void HandleUnloadChartMessage(ChartMessage message)
        {
            if (message.Charts != null)
            {
                if (map.Overlays.Contains(chartsOverlayName))
                {
                    LayerOverlay overlay = ((LayerOverlay)map.Overlays[chartsOverlayName]);
                    foreach (ChartItem item in message.Charts)
                    {
                        for (int i = overlay.Layers.Count - 1; i >= 0; i--)
                        {
                            Layer layer = overlay.Layers[i];
                            if (item.FileName == layer.Name)
                            {
                                overlay.Layers.Remove(layer);

                                boundingBoxPreviewLayer.InternalFeatures.Remove(layer.GetHashCode().ToString());
                                break;
                            }
                        }
                        if (ChartSelectedItem != null && ChartSelectedItem.FullName == item.FileName)
                        {
                            ChartSelectedItem = new ChartSelectedItem(string.Empty, null);
                        }
                    }

                    RectangleShape boundingBox = null;
                    foreach (Layer layer in overlay.Layers)
                    {
                        layer.Open();
                        if (boundingBox == null)
                        {
                            boundingBox = layer.GetBoundingBox();
                        }
                        else
                        {
                            boundingBox.ExpandToInclude(layer.GetBoundingBox());
                        }
                        layer.Close();
                    }
                    if (boundingBox != null)
                    {
                        map.CurrentExtent = boundingBox;
                    }
                    
                    map.Refresh();
                }
            }
        }

        private void hydrographyLayer_DrawingFeatures(object sender, DrawingFeaturesEventArgs e)
        {
            if (!IsHydrographyLayerVisiable())
            {
                e.Cancel = true;
            }
        }

        private void InitBoundingBoxPreviewOverlay(WpfMap map)
        {
            boundingBoxPreviewLayer = new InMemoryFeatureLayer();
            boundingBoxPreviewLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.StandardColors.Transparent, GeoColor.StandardColors.Blue);
            boundingBoxPreviewLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay boundingBoxPreviewOverlay = new LayerOverlay();
            boundingBoxPreviewOverlay.Layers.Add(boundingBoxPreviewLayer);
            map.Overlays.Add(boundingBoxPreviewOverlayName, boundingBoxPreviewOverlay);
        }

        private bool IsHydrographyLayerVisiable()
        {
            bool enable = false;

            if (boundingBoxPreviewLayer != null)
            {
                foreach (Feature f in boundingBoxPreviewLayer.InternalFeatures)
                {
                    if (f.GetBoundingBox().Width / map.CurrentExtent.Width > 0.5)
                    {
                        enable = true;
                        break;
                    }
                }
            }

            return enable;
        }

        private void LoadMessageHandlers()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        private void MenuItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Compare(e.PropertyName, "IsChecked", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                BaseMenuItem menuItem = sender as BaseMenuItem;
                if (menuItem.IsChecked)
                {
                    canHandleExecute = false;
                    switch (menuItem.GroupName.ToLowerInvariant())
                    {
                        case "s52areadrawingmode":
                            SelectedAreaDrawingMode = menuItem;
                            break;

                        case "colorschema":
                            SelectedColorSchema = menuItem;
                            break;

                        case "s52pointdrawingmode":
                            SelectedPointDrawingMode = menuItem;
                            break;

                        case "showworldmap":
                            SelectedBaseMap = menuItem;
                            break;

                        case "displaycategory":
                            SelectedDisplayCategory = menuItem;
                            break;

                        case "textlabel":
                            SelectedSymbolLabel = menuItem;
                            break;
                    }
                    canHandleExecute = true;
                }
            }
        }

        //private void overlay_Drawing(object sender, DrawingOverlayEventArgs e)
        //{
        //    if (IsHydrographyLayerVisiable())
        //    {
        //        IsOnLoading = true;
        //    }
        //}

        //private void overlay_Drawn(object sender, DrawnOverlayEventArgs e)
        //{
        //    IsOnLoading = false;
        //}

        private void SetToolbarMenuItems()
        {
            areaDrawingModes = GetMenusByGroupName("s52areadrawingmode", menuItems);
            foreach (BaseMenuItem menuItem in areaDrawingModes)
            {
                menuItem.PropertyChanged += MenuItem_PropertyChanged;
            }
            SelectedAreaDrawingMode = areaDrawingModes.FirstOrDefault(p => p.IsChecked);
            colorSchemas = GetMenusByGroupName("colorschema", menuItems);
            foreach (BaseMenuItem menuItem in colorSchemas)
            {
                menuItem.PropertyChanged += MenuItem_PropertyChanged;
            }
            SelectedColorSchema = colorSchemas.FirstOrDefault(p => p.IsChecked);
            pointDrawingModes = GetMenusByGroupName("s52pointdrawingmode", menuItems);
            foreach (BaseMenuItem menuItem in pointDrawingModes)
            {
                menuItem.PropertyChanged += MenuItem_PropertyChanged;
            }
            SelectedPointDrawingMode = pointDrawingModes.FirstOrDefault(p => p.IsChecked);

            baseMaps = GetMenusByGroupName("showworldmap", menuItems);
            foreach (BaseMenuItem menuItem in baseMaps)
            {
                menuItem.PropertyChanged += MenuItem_PropertyChanged;
            }
            SelectedBaseMap = baseMaps.FirstOrDefault(m => m.IsChecked);

            displayCategorys = GetMenusByGroupName("displaycategory", menuItems);
            foreach (BaseMenuItem menuItem in displayCategorys)
            {
                menuItem.PropertyChanged += MenuItem_PropertyChanged;
            }
            SelectedDisplayCategory = displayCategorys.FirstOrDefault(m => m.IsChecked);

            symbolLabels = GetMenusByGroupName("textlabel", menuItems);
            foreach (BaseMenuItem menuItem in symbolLabels)
            {
                menuItem.PropertyChanged += MenuItem_PropertyChanged;
            }
            SelectedSymbolLabel = symbolLabels.FirstOrDefault(p => p.IsChecked);

            showingGradicule = GetMenuByAction("graticule", menuItems);
            showLights = GetMenuByAction("lights", menuItems);

            showContourText = GetMenuByAction("contourlabel", menuItems);
            showSoundingText = GetMenuByAction("soundinglabel", menuItems);
            showLightDescriptions = GetMenuByAction("lightdescription", menuItems);
        }

        //private void SetupAnimationForOverlay(LayerOverlay overlay)
        //{
        //    overlay.Drawing -= overlay_Drawing;
        //    overlay.Drawing += overlay_Drawing;
        //    overlay.Drawn -= overlay_Drawn;
        //    overlay.Drawn += overlay_Drawn;
        //}

        private void WpfMap_MapClick(object sender, MapClickWpfMapEventArgs e)
        {
            if (isIdentify)
            {
                PointShape point = e.WorldLocation;
                if (!map.Overlays.Contains(chartsOverlayName))
                {
                    return;
                }
                LayerOverlay overlay = map.Overlays[chartsOverlayName] as LayerOverlay;

                var features = new Collection<Feature>();
                NauticalChartsFeatureLayer hydrographyFeatureLayer = null;
                foreach (var item in overlay.Layers)
                {
                    NauticalChartsFeatureLayer itemLayer = item as NauticalChartsFeatureLayer;
                    itemLayer.Open();
                    features = itemLayer.QueryTools.GetFeaturesIntersecting(point.GetBoundingBox(), ReturningColumnsType.AllColumns);

                    if (features.Count > 0)
                    {
                        hydrographyFeatureLayer = itemLayer;
                        break;
                    }
                }

                if (features.Count > 0)
                {
                    List<FeatureInfo> selectedFeatures = new List<FeatureInfo>();

                    foreach (var item in features)
                    {
                        double area = double.MaxValue;
                        PolygonShape areaShape = item.GetShape() as PolygonShape;
                        if (areaShape != null)
                        {
                            area = areaShape.GetArea(map.MapUnit, AreaUnit.SquareMeters);
                        }
                        selectedFeatures.Add(new FeatureInfo(item, hydrographyFeatureLayer.Name, area));
                    }

                    if (map.Overlays.Contains(highlightOverlayName))
                    {
                        map.Overlays.Remove(highlightOverlayName);
                    }

                    IEnumerable<FeatureInfo> featureInfos = selectedFeatures.OrderBy(p => p.Area);
                    SelectedFeatureInfo = featureInfos.FirstOrDefault();
                    NauticalChartsFeatureSource featureSource = hydrographyFeatureLayer.FeatureSource as NauticalChartsFeatureSource;
                    if (featureSource != null)
                    {
                        ChartSelectedItem = new ChartSelectedItem(featureSource.NauticalChartsPathFilename, featureInfos);
                    }
                }
                else
                {
                    if (map.Overlays.Contains(highlightOverlayName))
                    {
                        map.Overlays.Remove(highlightOverlayName);
                    }
                    map.Refresh();
                }
            }
        }
    }
}