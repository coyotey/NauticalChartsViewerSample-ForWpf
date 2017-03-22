using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{
    public class ChartsManagmentViewModel : ViewModelBase
    {
        private BackgroundWorker buildIndexWorker;
        private ObservableCollection<ChartItem> charts;
        private bool isProgressBarVisible;
        private ICommand doubleClickCommand; 
        private ICommand cancelCommand;
        private ICommand loadCommand;
        private ICommand loadDirectoryCommand;
        private ICommand okCommand;  
        private ICommand unloadCommand;
        private ChartItem selectedItem;
        private Collection<ChartItem> selectedItems;
        private static ChartsManagmentViewModel instance;

        private ChartsManagmentViewModel()
        {
            charts = new ObservableCollection<ChartItem>();
            selectedItems = new Collection<ChartItem>();
            buildIndexWorker = new BackgroundWorker();
            buildIndexWorker.DoWork += buildIndexWorker_DoWork;
            buildIndexWorker.RunWorkerCompleted += buildIndexWorker_RunWorkerCompleted;
        }

        public static ChartsManagmentViewModel Instance
        {
            get
            {
                return instance ?? (instance = new ChartsManagmentViewModel());
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new RelayCommand(HandleCancelCommand, () => !buildIndexWorker.IsBusy));
            }
        }

        public ObservableCollection<ChartItem> Charts
        {
            get { return charts; }
        }

        public ICommand DoubleClickCommand
        {
            get
            {
                return doubleClickCommand ?? (doubleClickCommand = new RelayCommand(HandleDoubleClickCommand, () => !buildIndexWorker.IsBusy));
            }
        }

        public bool IsProgressBarVisible
        {
            get { return isProgressBarVisible; }
            set
            {
                if (isProgressBarVisible != value)
                {
                    isProgressBarVisible = value;
                    RaisePropertyChanged("IsProgressBarVisible");
                }
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return loadCommand ?? (loadCommand = new RelayCommand(HandleLoadCommand, () => !buildIndexWorker.IsBusy));
            }
        }

        public ICommand LoadDirectoryCommand
        {
            get
            {
                return loadDirectoryCommand ?? (loadDirectoryCommand = new RelayCommand(HandleLoadDirectoryCommand, () => !buildIndexWorker.IsBusy));
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return okCommand ?? (okCommand = new RelayCommand(HandleOkCommand, () => !buildIndexWorker.IsBusy && selectedItems.Count > 0));
            }
        }

        public ChartItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }
        }

        public Collection<ChartItem> SelectedItems
        {
            get { return selectedItems; }
        }

        public ICommand UnloadCommand
        {
            get
            {
                return unloadCommand ?? (unloadCommand = new RelayCommand(HandleUnloadCommand, () => selectedItems.Count > 0 && !buildIndexWorker.IsBusy));
            }
        }

        public override void Cleanup()
        {
            if (buildIndexWorker != null && !buildIndexWorker.IsBusy)
            {
                buildIndexWorker.Dispose();
            }
            base.Cleanup();
        }

        private void buildIndexWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IEnumerable<string> fileNames = e.Argument as IEnumerable<string>;
            foreach (string fileName in fileNames)
            {
                NauticalChartsFeatureSource.BuildIndexFile(fileName, BuildIndexMode.Rebuild);
            }
            e.Result = fileNames;
        }

        private void buildIndexWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IEnumerable<string> fileNames = e.Result as IEnumerable<string>;

            bool succeed = true;
            foreach (string fileName in fileNames)
            {
                if (!ExistsIndexFile(fileName))
                {
                    succeed = false;
                    MessageBox.Show("Failed to build index", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (Charts.Count(p => p.FileName == fileName) == 0)
                    {
                        ChartItem item = new ChartItem(fileName);
                        Charts.Add(item);
                    }
                }
            }
            if (charts.Count > 0)
            {
                SelectedItem = Charts[0];
            }
            IsProgressBarVisible = false;
            if (succeed)
            {
                MessageBox.Show("Index building compeleted", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool ExistsIndexFile(string s52fileName)
        {
            string fileNameWithoutEx = Path.GetFileNameWithoutExtension(s52fileName);
            string indexFileName = Path.Combine(Path.GetDirectoryName(s52fileName), string.Format("{0}.idx", fileNameWithoutEx));
            string s52IndexFileName = Path.ChangeExtension(indexFileName, ".hyr");
            return (File.Exists(indexFileName) && File.Exists(s52IndexFileName));
        }

        private void HandleCancelCommand()
        {
            Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close));
        }

        private void HandleDoubleClickCommand()
        {
            if (SelectedItem == null)
            {
                return;
            }

            ChartMessage message = new ChartMessage(new[] { SelectedItem });

            Messenger.Default.Send<ChartMessage>(message, "LoadCharts");
        }

        private void HandleLoadCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.000)|*.000";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                if (!ExistsIndexFile(fileName))
                {
                    MessageBoxResult result = MessageBox.Show("Index file does not exists, do you want to create?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    IsProgressBarVisible = true;
                    buildIndexWorker.RunWorkerAsync(new[] { fileName });
                }
                else
                {
                    ChartItem item = Charts.FirstOrDefault(p => p.FileName == fileName);
                    if (item == null)
                    {
                        item = new ChartItem();
                        item.FileName = fileName;
                        Charts.Add(item);
                    }

                    SelectedItem = item;
                }
            }
        }

        private void HandleLoadDirectoryCommand()
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string directory = folderBrowserDialog.SelectedPath;
                Collection<string> files = new Collection<string>();
                LoadFileRecursively(directory, files);
                Collection<string> filesWithIndexFile = new Collection<string>();
                Collection<string> filesWithoutIndexFile = new Collection<string>();
                foreach (string file in files)
                {
                    if (!ExistsIndexFile(file))
                    {
                        filesWithoutIndexFile.Add(file);
                    }
                    else
                    {
                        filesWithIndexFile.Add(file);
                    }
                }
                if (filesWithoutIndexFile.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Index file does not exists, do you want to create?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }
                    IsProgressBarVisible = true;
                    buildIndexWorker.RunWorkerAsync(filesWithoutIndexFile);
                }

                foreach (string fileName in filesWithIndexFile)
                {
                    ChartItem item = new ChartItem(fileName);
                    Charts.Add(item);
                }
                if (charts.Count > 0)
                {
                    SelectedItem = Charts[0];
                }
            }
        }

        private void HandleOkCommand()
        {
            ChartMessage message = new ChartMessage(selectedItems);
            Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close));
            Messenger.Default.Send<ChartMessage>(message, "LoadCharts");
            selectedItems.Clear();
        }

        private void HandleUnloadCommand()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure remove selected charts?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ChartMessage message = new ChartMessage(selectedItems);
                Messenger.Default.Send<ChartMessage>(message, "UnloadCharts");
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                {
                    ChartItem chart = selectedItems[i];
                    Charts.Remove(chart);
                }
            }
        }

        private void LoadFileRecursively(string directoryPath, Collection<string> candidates)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.000");
            foreach (string file in files)
            {
                candidates.Add(file);
            }
            string[] directories = Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                LoadFileRecursively(directory, candidates);
            }
        }
    }
}