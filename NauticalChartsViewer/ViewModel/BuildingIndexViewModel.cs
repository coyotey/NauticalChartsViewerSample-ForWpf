using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{
    public class BuildingIndexViewModel : ViewModelBase
    {
        private ICommand browseCommand;
        private ICommand buildCommand;
        private ICommand cancelCommand;
        private string fileName;
        private string indexFileName;
        private bool isProgressBarVisible;
        private bool rebuild; 
        private int progressPercentage;
        private BackgroundWorker worker;

        /// <summary>
        /// Initializes a new instance of the BuildingIndexViewModel class.
        /// </summary>
        public BuildingIndexViewModel()
        {
            worker = new BackgroundWorker() { WorkerReportsProgress = true };
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.DoWork += worker_DoWork;

            rebuild = true;
        }

        public ICommand BrowseCommand
        {
            get { return browseCommand ?? (browseCommand = new RelayCommand(HandBrowseCommand, () => !worker.IsBusy)); }
        }

        public ICommand BuildCommand
        {
            get { return buildCommand ?? (buildCommand = new RelayCommand(HandBuildCommand, () => !string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(IndexFileName) && !worker.IsBusy)); }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand ?? (cancelCommand = new RelayCommand(HandCancelCommand, () => !worker.IsBusy)); }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    string fileNameWithoutEx = Path.GetFileNameWithoutExtension(fileName);
                    IndexFileName = Path.Combine(Path.GetDirectoryName(fileName), string.Format("{0}.idx", fileNameWithoutEx));
                    RaisePropertyChanged("FileName");
                }
            }
        }

        public string IndexFileName
        {
            get { return indexFileName; }
            private set
            {
                if (indexFileName != value)
                {
                    indexFileName = value;
                    RaisePropertyChanged("IndexFileName");
                }
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

        public int ProgressPercentage
        {
            get { return progressPercentage; }
            set
            {
                if (progressPercentage != value)
                {
                    progressPercentage = value;
                    RaisePropertyChanged("ProgressPercentage");
                }
            }
        }

        public bool Rebuild
        {
            get { return rebuild; }
            set
            {
                if (rebuild != value)
                {
                    rebuild = value;
                    RaisePropertyChanged(() => Rebuild);
                }
            }
        }

        public override void Cleanup()
        {
            if (worker != null && !worker.IsBusy) { }
            {
                worker.Dispose();
            }

            base.Cleanup();
        }

        private void HandBrowseCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.000)|*.000|All files(*.*)|*.*";
            string fileName = string.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                FileName = fileName;
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void HandBuildCommand()
        {
            IsProgressBarVisible = true;
            worker.RunWorkerAsync();

            CommandManager.InvalidateRequerySuggested();
        }

        private void HandCancelCommand()
        {
            Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close), "BuildIndexWindow");
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (ExistsIndexFile(IndexFileName))
            {
                string s52IndexFileName = Path.ChangeExtension(IndexFileName, ".hyr");
                FileInfo s52IndexFile = new FileInfo(s52IndexFileName);
                s52IndexFile.Attributes = FileAttributes.Normal;
                s52IndexFile.Delete();

                FileInfo indexFile = new FileInfo(IndexFileName);
                indexFile.Attributes = FileAttributes.Normal;
                indexFile.Delete();
            }
            NauticalChartsFeatureSource.BuildIndexFile(FileName, Rebuild ? BuildIndexMode.Rebuild : BuildIndexMode.DoNotRebuild);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressPercentage = 0;
            IsProgressBarVisible = false;
            if (!e.Cancelled && e.Error == null)
            {
                if (ExistsIndexFile(IndexFileName))
                {
                    MessageBox.Show("Index building compeleted", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                    Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close), "BuildIndexWindow");
                }
                else
                {
                    MessageBox.Show("Failed to build index", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if(e.Error != null)
            {
                MessageBox.Show(e.Error.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ExistsIndexFile(string indexFilename)
        {
            string s52IndexFileName = Path.ChangeExtension(indexFilename, ".hyr");
            return (File.Exists(indexFilename) && File.Exists(s52IndexFileName));
        }

    }
}