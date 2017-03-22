using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{

    public class ColorTableViewModel : ViewModelBase
    {
        private NauticalChartsDefaultColorSchema selectedColorSchema;
        private readonly ObservableCollection<NauticalChartsDefaultColorSchema> colorSchemas;
        private readonly ObservableCollection<ColorItem> colorItems;
        private ColorItem selectedColorItem;
        private ColorItem editionColorItem;
        private Dictionary<NauticalChartsDefaultColorSchema, Collection<ColorEntry>> colorTables;
        private ICommand searchCommand;
        private ICommand commitCommand;
        private string searchText;
        private S52ResourceFilesAnalyst s52ResourceFileAnalyst;
        private bool isEnalbed;

        internal ColorTableViewModel() 
        { 
        }

        public ColorTableViewModel(S52ResourceFilesAnalyst s52ResourceFileAnalyst)
        {
            this.s52ResourceFileAnalyst = s52ResourceFileAnalyst;
            colorSchemas = new ObservableCollection<NauticalChartsDefaultColorSchema>();
            colorItems = new ObservableCollection<ColorItem>();
            IsEnabled = true;
            LoadColorTables();
            LoadCurrentSchemaColorItems();
            SelectedColorItem = colorItems.Count > 0 ? colorItems[0] : null; 
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

        public ColorItem SelectedColorItem
        {
            get { return selectedColorItem; }
            set
            {
                if (selectedColorItem != value)
                {
                    selectedColorItem = value;
                    EditionColorItem = value != null ? (ColorItem)value.Clone() : null;
                    RaisePropertyChanged("SelectedColorItem");
                }
            }
        }

        public ColorItem EditionColorItem
        {
            get { return editionColorItem; }
            set
            {
                if (editionColorItem != value)
                {
                    editionColorItem = value;
                    RaisePropertyChanged("EditionColorItem");
                }
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; }
        }

        public ObservableCollection<NauticalChartsDefaultColorSchema> ColorSchemas
        {
            get { return colorSchemas; }
        }

        public ObservableCollection<ColorItem> ColorItems
        {
            get { return colorItems; }
        }


        public NauticalChartsDefaultColorSchema SelectedColorSchema
        {
            get { return selectedColorSchema; }
            set
            {
                if (selectedColorSchema != value)
                {
                    selectedColorSchema = value;
                    LoadCurrentSchemaColorItems();
                    RaisePropertyChanged("SelectedColorSchema");
                }
            }
        }


        public ICommand SearchCommand
        {
            get
            {
                return searchCommand ?? (searchCommand = new RelayCommand(HandleSearchCommand, () => IsEnabled));
            }
        }

        private void HandleSearchCommand()
        {
            if (searchText != null)
            {
                Collection<ColorEntry> colorEntries = colorTables[selectedColorSchema];
                colorItems.Clear();
                foreach (ColorEntry entry in colorEntries)
                {
                    if ((!string.IsNullOrEmpty(entry.Token.ToUpperInvariant()) && entry.Token.Contains(searchText.ToUpperInvariant())) ||
                        (!string.IsNullOrEmpty(entry.Name.ToUpperInvariant()) && entry.Name.Contains(searchText.ToUpperInvariant())))
                    {
                        colorItems.Add(new ColorItem(entry.Name, entry.Token, entry.Color.RedComponent, entry.Color.GreenComponent, entry.Color.BlueComponent));
                    }
                }
            }
        }

        public ICommand CommitCommand
        {
            get { return commitCommand ?? (commitCommand = new RelayCommand(HandleCommmitCommand, () => EditionColorItem != null)); }
        }


        private ICommand showColorPicker;
        public ICommand ShowColorPicker
        {
            get
            {
                return showColorPicker ?? (showColorPicker = new RelayCommand(() =>
                {
                    System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                    if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        System.Drawing.Color color = colorDialog.Color;
                        editionColorItem.R = color.R;
                        editionColorItem.G = color.G;
                        editionColorItem.B = color.B;
                    }
                }, () => EditionColorItem != null));
            }
        }

        private void HandleCommmitCommand()
        {
            ColorEntry newColorEntry = new ColorEntry(editionColorItem.Token, editionColorItem.Name, editionColorItem.R, editionColorItem.G, editionColorItem.B);
            s52ResourceFileAnalyst.UpdateColorEntry(selectedColorSchema, newColorEntry);
            for (int i = 0; i < colorTables[selectedColorSchema].Count; i++)
            {
                ColorEntry item = colorTables[selectedColorSchema][i];
                if (item.Token == editionColorItem.Token)
                {
                    colorTables[selectedColorSchema][i] = newColorEntry;
                    break;
                }
            }
            LoadCurrentSchemaColorItems();
            MessageBox.Show("Commit succeed.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadColorTables()
        {
            colorTables = this.s52ResourceFileAnalyst.GetColorEntries();
            colorSchemas.Clear();
            foreach (var key in colorTables.Keys)
            {
                colorSchemas.Add(key);
            }
        }

        private void LoadCurrentSchemaColorItems()
        {
            colorItems.Clear();
            Collection<ColorEntry> colorEntries = colorTables[selectedColorSchema];
            foreach (ColorEntry entry in colorEntries)
            {
                colorItems.Add(new ColorItem(entry.Name, entry.Token, entry.Color.RedComponent, entry.Color.GreenComponent, entry.Color.BlueComponent));
            }
        }
    }

}