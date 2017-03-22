using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{
    public class SymbolTableViewModel : ViewModelBase
    {
        private NauticalChartsDefaultColorSchema selectColorSchema;
        private string searchSymbol;
        private SymbolType selectedSymbolType;
        private SymbolItem selectedSymbolItem;
        private SymbolItem editionSymbolItem;
        private ImageSource symbolBitmapImage;
        private ICommand searchCommand;
        private ICommand commitCommand;
        private Dictionary<SymbolType, Collection<ChartStyleModule>> chartTables;
        private ObservableCollection<SymbolItem> symbolItems;
        private ObservableCollection<SymbolType> symbolStyleSchemas;
        private S52ResourceFilesAnalyst s52ResourceFilesAnalyst;
        private Dictionary<NauticalChartsDefaultColorSchema, Collection<ColorEntry>> colorTables;
        private readonly Dictionary<SymbolType, S52ObjectType> symbolTypeObjectTypeMappings = new Dictionary<SymbolType, S52ObjectType> 
        {
            {SymbolType.Symbols, S52ObjectType.Symbol},
            {SymbolType.Lines, S52ObjectType.ComplexLineStyle},
            {SymbolType.Patterns, S52ObjectType.Pattern}
        };

        private bool isEnalbed;
        private const int imageWidth = 200;
        private const int imageHeight = 200;

        internal SymbolTableViewModel()
        {
        }

        public SymbolTableViewModel(S52ResourceFilesAnalyst s52ResourceFilesAnalyst, NauticalChartsDefaultColorSchema colorSchema)
        {
            this.s52ResourceFilesAnalyst = s52ResourceFilesAnalyst;
            symbolItems = new ObservableCollection<SymbolItem>();
            symbolStyleSchemas = new ObservableCollection<SymbolType>();
            selectColorSchema = colorSchema;
            IsEnabled = true;
            chartTables = this.s52ResourceFilesAnalyst.GetSymbolModules();
            colorTables = this.s52ResourceFilesAnalyst.GetColorEntries();
            LoadSymbolStyleSchemas();
            LoadSymbolList();
            SelectedSymbolItem = symbolItems.Count > 0 ? symbolItems[0] : null;
        }

        public NauticalChartsDefaultColorSchema SelectColorSchem
        {
            get { return selectColorSchema; }
            set
            {
                if (selectColorSchema != value)
                {
                    selectColorSchema = value;
                    if (editionSymbolItem != null)
                    {
                        DrawSymbols();
                    }
                }
            }
        }

        public bool IsEnabled
        {
            get { return isEnalbed; }
            set
            {
                if (isEnalbed != value)
                {
                    isEnalbed = value;
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }

        public string SearchSymbol
        {
            get { return searchSymbol; }
            set { searchSymbol = value; }
        }

        public SymbolType SelectedSymbolType
        {
            get { return selectedSymbolType; }
            set
            {
                if (selectedSymbolType != value)
                {
                    selectedSymbolType = value;
                    LoadSymbolList();
                    RaisePropertyChanged("SelectedSymbolType");
                }
            }
        }

        public SymbolItem SelectedSymbolItem
        {
            get { return selectedSymbolItem; }
            set
            {
                if (selectedSymbolItem != value)
                {
                    selectedSymbolItem = value;
                    EditionSymbolItem = value != null ? (SymbolItem)value.Clone() : null;
                    RaisePropertyChanged("SelectedSymbolItem");
                }
            }
        }

        public SymbolItem EditionSymbolItem
        {
            get { return editionSymbolItem; }
            set
            {
                if (editionSymbolItem != value)
                {
                    editionSymbolItem = value;
                    DrawSymbols();
                    if (value == null)
                    {
                        SymbolBitmapImage = null;
                    }
                    RaisePropertyChanged("EditionSymbolItem");
                }
            }
        }

        public ImageSource SymbolBitmapImage
        {
            get { return symbolBitmapImage; }
            set
            {
                if (symbolBitmapImage != value)
                {
                    symbolBitmapImage = value;
                    RaisePropertyChanged("SymbolBitmapImage");
                }
            }
        }

        public ICommand SearchCommand
        {
            get { return searchCommand ?? (searchCommand = new RelayCommand(HandleSearchSymbolCommand, () => IsEnabled)); }
        }

        private void HandleSearchSymbolCommand()
        {
            if (searchSymbol != null)
            {
                symbolItems.Clear();
                chartTables[selectedSymbolType].Where(p => p.Name.Contains(searchSymbol.ToUpperInvariant())).
                    Select(p => new SymbolItem(p.Rcid, p.BoundingBox, p.ColorReferences, p.Commands, p.CommandType, p.Name, p.Pivot, p.UpperLeft)).
                        ToList().ForEach(symbolItems.Add);
                SelectedSymbolItem = symbolItems.FirstOrDefault();
            }
        }

        public ICommand CommitCommand
        {
            get { return commitCommand ?? (commitCommand = new RelayCommand(HandleCommitCommand, () => EditionSymbolItem != null)); }
        }

        private void HandleCommitCommand()
        {
            ChartStyleModule item = chartTables[selectedSymbolType].First(p => p.Rcid == editionSymbolItem.Rcid);
            item.Commands.Clear();
            foreach (EditionStringField command in editionSymbolItem.EditionCommands)
            {
                item.Commands.Add(command.StringField);
            }
            s52ResourceFilesAnalyst.UpdateChartTableEntry(selectedSymbolType, item);
            SymbolItem selectedItemCache = SelectedSymbolItem;
            LoadSymbolList();
            SelectedSymbolItem = selectedItemCache;
            MessageBox.Show("Commit succeed.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public ObservableCollection<SymbolItem> SymbolItems
        {
            get { return symbolItems; }
            set { symbolItems = value; }
        }

        public ObservableCollection<SymbolType> SymbolStyleSchemas
        {
            get { return symbolStyleSchemas; }
            set { symbolStyleSchemas = value; }
        }


        private void LoadSymbolStyleSchemas()
        {
            SymbolStyleSchemas.Clear();
            foreach (SymbolType key in chartTables.Keys)
            {
                SymbolStyleSchemas.Add(key);
            }
        }

        private void LoadSymbolList()
        {
            symbolItems.Clear();
            foreach (ChartStyleModule item in chartTables[selectedSymbolType])
            {
                symbolItems.Add(new SymbolItem(item.Rcid, item.BoundingBox, item.ColorReferences, item.Commands,
                    item.CommandType, item.Name, item.Pivot, item.UpperLeft));
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            bitmap.Save("filePath");
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hBitmap);
            return wpfBitmap;
        }

        private void DrawSymbols()
        {
            if (editionSymbolItem != null)
            {
                Dictionary<string, RGBColor> colorRef = new Dictionary<string, RGBColor>();
                Collection<ColorEntry> colorEntries = colorTables[selectColorSchema];
                RGBColor backgroundColor = new RGBColor();
                foreach (var item in colorEntries)
                {
                    if (item.Token.Equals("NODTA"))
                    {
                        backgroundColor = new RGBColor(item.Color.RedComponent, item.Color.GreenComponent, item.Color.BlueComponent);
                    }
                    colorRef.Add(item.Token, new RGBColor(item.Color.RedComponent, item.Color.GreenComponent, item.Color.BlueComponent));

                }

                Collection<string> vectorCommands = new Collection<string>();

                foreach (string content in editionSymbolItem.Commands)
                {
                    Collection<string> subcontents = S52RegularInterpolator.GetSubcontentsBySplitChar(content, ';');

                    foreach (string subcontent in subcontents)
                    {
                        vectorCommands.Add(subcontent);
                    }
                }

                Dictionary<string, RGBColor> detailedcolorRef = new Dictionary<string, RGBColor>();
                foreach (var item in editionSymbolItem.ColorReferences)
                {
                    string content = item.Value;
                    RGBColor color = colorRef[content];
                    detailedcolorRef.Add(item.Key.ToString(), color);
                }

                VectorCommandField vectorCommandField = new VectorCommandField(vectorCommands);
                vectorCommandField.GetDrawingShapes(detailedcolorRef);

                S52Object symbolObject = new S52Object();
                symbolObject.ObjectType = symbolTypeObjectTypeMappings[selectedSymbolType];
                symbolObject.Height = editionSymbolItem.BoundingBox.Height;
                symbolObject.Width = editionSymbolItem.BoundingBox.Width;
                symbolObject.PivotVertex = new ThinkGeo.MapSuite.Vertex(editionSymbolItem.Pivot.X, editionSymbolItem.Pivot.Y);
                symbolObject.UpperLeftVertex = new ThinkGeo.MapSuite.Vertex(editionSymbolItem.UpperLeft.X, editionSymbolItem.UpperLeft.Y);
                symbolObject.Shapes = vectorCommandField.GetDrawingShapes(detailedcolorRef);

                using (S52ObjectsPresenter s52ObjectsPrinter = new S52ObjectsPresenter(imageWidth, imageHeight))
                {
                    s52ObjectsPrinter.Clear(backgroundColor);
                    s52ObjectsPrinter.Draw(symbolObject, backgroundColor);
                    SymbolBitmapImage = ChangeBitmapToImageSource(s52ObjectsPrinter.GetBitmap());
                }
            }
        }

    }
}
