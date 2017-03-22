using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ThinkGeo.MapSuite;

namespace NauticalChartsViewer
{
    public class LookupTableViewModel : ViewModelBase
    {
        private S52ResourceFilesAnalyst s52ResourceFilesAnalyst;
        private ObservableCollection<LookupItem> lookupItems;
        private Dictionary<LookupTableType, Collection<LookupTableModule>> lookupTables;
        private readonly ObservableCollection<LookupTableType> lookupTableTypes;
        private LookupTableType selectedLookupTableType;
        private LookupItem selectedLookupItem;
        private LookupItem editionLookupItem;
        private string searchLookup;
        private ICommand searchCommand;
        private ICommand commitCommand;
        private bool isEnalbed;

        internal LookupTableViewModel()
        {
        }

        public LookupTableViewModel(S52ResourceFilesAnalyst s52ResourceFilesAnalyst)
        {
            lookupItems = new ObservableCollection<LookupItem>();
            lookupTableTypes = new ObservableCollection<LookupTableType>();
            this.s52ResourceFilesAnalyst = s52ResourceFilesAnalyst;
            IsEnabled = true;
            LoadLookupTables();
            LoadLookupItems();
            SelectedLookupItem = lookupItems.Count > 0 ? lookupItems[0] : null; 
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

        public LookupItem SelectedLookupItem
        {
            get { return selectedLookupItem; }
            set
            {
                if (selectedLookupItem != value)
                {
                    selectedLookupItem = value;
                    EditionLookupItem = value != null ? (LookupItem)value.Clone() : null;
                }
            }
        }

        public LookupItem EditionLookupItem
        {
            get { return editionLookupItem; }
            set
            {
                if (editionLookupItem != value)
                {
                    editionLookupItem = value;
                    RaisePropertyChanged("EditionLookupItem");
                }
            }
        }

        public LookupTableType SelectedLookupTableType
        {
            get { return selectedLookupTableType; }
            set
            {
                if (selectedLookupTableType != value)
                {
                    selectedLookupTableType = value;
                    LoadLookupItems();
                    RaisePropertyChanged("SelectedLookupTableType");
                }
            }
        }

        public ObservableCollection<LookupTableType> LookupTableTypes
        {
            get { return lookupTableTypes; }
        }

        public ObservableCollection<LookupItem> LookupItems
        {
            get { return lookupItems; }
        }

        public string SearchLookup
        {
            get { return searchLookup; }
            set { searchLookup = value; }
        }
        
        public ICommand SearchCommand
        {
            get { return searchCommand ?? (searchCommand = new RelayCommand(HandleSearchLookupItemCommand, () => IsEnabled)); }
        }

        public ICommand CommitCommand
        {
            get { return commitCommand ?? (commitCommand = new RelayCommand(HandleCommitCommand, () => EditionLookupItem != null)); }
        }

        private void HandleCommitCommand()
        {
            LookupTableModule item = lookupTables[selectedLookupTableType].First(p => p.Rcid == editionLookupItem.Rcid);
            Collection<string> instructions = new Collection<string>();
            foreach (EditionStringField instruction in editionLookupItem.EditionInstructions)
            {
                instructions.Add(instruction.StringField);
            }
            item.Instructions = instructions;
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            foreach (EditionKeyValue attribute in editionLookupItem.EditionAttributes)
            {
                attributes.Add(attribute.Key, attribute.Value);
            }
            item.Attributes = attributes;
            item.DisplayPriority = editionLookupItem.DisplayPriority;
            item.RadarPriority = editionLookupItem.RadarPriority;
            item.DisplayCategory = editionLookupItem.DisplayCategory;
            s52ResourceFilesAnalyst.UpdateLookupTableEntry(selectedLookupTableType, item);
            LoadLookupItems();
            MessageBox.Show("Commit succeed.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadLookupTables()
        {
            lookupTables = s52ResourceFilesAnalyst.GetLookupEntries();
            lookupTableTypes.Clear();
            foreach (var key in lookupTables.Keys)
            {
                lookupTableTypes.Add(key);
            }
        }

        private void HandleSearchLookupItemCommand()
        {
            if (searchLookup != null)
            {
                lookupItems.Clear();
                lookupTables[selectedLookupTableType].Where(p => !string.IsNullOrEmpty(p.ObjectClass) && p.ObjectClass.Contains(searchLookup.ToUpperInvariant())).
                     Select(p => new LookupItem(
                        p.Rcid,
                        p.ObjectClass,
                        p.DisplayPriority,
                        p.RadarPriority,
                        p.DisplayType,
                        p.Attributes,
                        p.Instructions,
                        p.DisplayCategory,
                        p.Comments)).
                    ToList().ForEach(lookupItems.Add);
                SelectedLookupItem = lookupItems.FirstOrDefault();
            }
        }

        private void LoadLookupItems()
        {
            lookupItems.Clear();
            lookupTables[selectedLookupTableType].
                Select(p => new LookupItem(
                    p.Rcid,
                    p.ObjectClass,
                    p.DisplayPriority,
                    p.RadarPriority,
                    p.DisplayType,
                    p.Attributes,
                    p.Instructions,
                    p.DisplayCategory,
                    p.Comments)).
                ToList().ForEach(lookupItems.Add);
        }
    }
}
