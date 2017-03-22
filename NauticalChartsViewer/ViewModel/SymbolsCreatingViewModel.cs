using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{

    public class SymbolsCreatingViewModel : ViewModelBase
    {
        private string daiFilePath;
        private string outputFilePath;
        private bool isRebuild;
        private bool isProgressBarVisible;
        private ICommand createCommand;
        private ICommand cancelCommand;
        private ICommand daiBrowerCommand;
        private ICommand savePathCommand;
        private BackgroundWorker backgroundWorker;

        public SymbolsCreatingViewModel()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
        }

        public string DaiFilePath
        {
            get { return daiFilePath; }
            set
            {
                if (daiFilePath != value)
                {
                    daiFilePath = value;
                    if (!string.IsNullOrEmpty(daiFilePath))
                    {
                        OutputFilePath = Path.GetDirectoryName(daiFilePath) + @"\NauticalCharts.xml";
                    }
                    else
                    {
                        OutputFilePath = string.Empty;
                    }
                    RaisePropertyChanged("DaiFilePath");
                }
            }
        }

        public string OutputFilePath
        {
            get { return outputFilePath; }
            set
            {
                if (outputFilePath != value)
                {
                    outputFilePath = value;
                    RaisePropertyChanged("OutputFilePath");
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

        public bool IsRebuild
        {
            get { return isRebuild; }
            set
            {
                if (isRebuild != value)
                {
                    isRebuild = value;
                    RaisePropertyChanged("IsRebuild");
                }
            }
        }

        public ICommand DAIFileBrowerCommand
        {
            get
            {
                return daiBrowerCommand ?? (daiBrowerCommand = new RelayCommand(HandleBrowerDAIFileCommand, () => !backgroundWorker.IsBusy));
            }
        }

        private void HandleBrowerDAIFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.dai)|*.dai";

            if (openFileDialog.ShowDialog() == true)
            {
                DaiFilePath = openFileDialog.FileName;
            }
        }

        public ICommand CreateCommand
        {
            get
            {
                return createCommand ?? (createCommand = new RelayCommand(HandleCreateCommand, () =>
                    !string.IsNullOrEmpty(DaiFilePath) &&
                    !string.IsNullOrEmpty(OutputFilePath) &&
                    !backgroundWorker.IsBusy));
            }
        }

        private void HandleCreateCommand()
        {
            IsProgressBarVisible = true;
            backgroundWorker.RunWorkerAsync();
        }

        public ICommand SavePathCommand
        {
            get
            {
                return savePathCommand ?? (savePathCommand = new RelayCommand(HandleSavePathCommand, () => !backgroundWorker.IsBusy));
            }
        }

        private void HandleSavePathCommand()
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "S52 Symbol File(*.xml)|*.xml",
                FileName = "NauticalCharts.xml"
            };
            if (dialog.ShowDialog() ?? true)
            {
                OutputFilePath = dialog.FileName;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new RelayCommand(HandleCancelCommand, () => !backgroundWorker.IsBusy));
            }
        }

        private void HandleCancelCommand()
        {
            Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close), "SymbolsCreatingWindow");
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            NauticalChartsFeatureLayer.CreateNauticalChartsStyleFileFromIhoDaiFile(daiFilePath, outputFilePath);
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsProgressBarVisible = false;
            if (!e.Cancelled && e.Error == null)
            {
                MessageBox.Show("Symbol file creating completed.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close), "SymbolsCreatingWindow");
            }
            else if (e.Error != null)
            {
                if (e.Error is XmlException)
                {
                    MessageBox.Show("An exception occurred during creating symbol file.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(e.Error.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        public override void Cleanup()
        {
            if (backgroundWorker != null && !backgroundWorker.IsBusy)
            {
                backgroundWorker.Dispose();
            }
            base.Cleanup();
        }

    }
}