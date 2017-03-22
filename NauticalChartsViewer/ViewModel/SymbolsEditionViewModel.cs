using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using ThinkGeo.MapSuite;

namespace NauticalChartsViewer
{

    public class SymbolsEditionViewModel : ViewModelBase
    {
        private string s52SymbolsSourcePath;
        private ColorTableViewModel colorTableViewModel;
        private SymbolTableViewModel symbolTableViewModel;
        private LookupTableViewModel lookupTableViewModel;
        private S52ResourceFilesAnalyst s52ResourceFilesAnalyst;
        private ICommand cancelCommand; 
        private ICommand s52ResourceBrowerCommand;

        public SymbolsEditionViewModel()
        {
            colorTableViewModel = new ColorTableViewModel();
            symbolTableViewModel = new SymbolTableViewModel();
            lookupTableViewModel = new LookupTableViewModel();
        }

        public string S52SymbolsSourcePath
        {
            get { return s52SymbolsSourcePath; }
            set
            {
                if (s52SymbolsSourcePath != value)
                {
                    try
                    {
                        s52ResourceFilesAnalyst = new S52ResourceFilesAnalyst(value);
                        s52SymbolsSourcePath = value;
                        ColorTable = new ColorTableViewModel(s52ResourceFilesAnalyst);
                        ColorTable.PropertyChanged += (sender, e) =>
                        {
                            if (e.PropertyName == "SelectedColorSchema")
                            {
                                symbolTableViewModel.SelectColorSchem = colorTableViewModel.SelectedColorSchema;
                            }
                        };
                        SymbolTable = new SymbolTableViewModel(s52ResourceFilesAnalyst, ColorTable.SelectedColorSchema);
                        LookupTable = new LookupTableViewModel(s52ResourceFilesAnalyst);
                        RaisePropertyChanged("S52SymbolsSourcePath");
                    }
                    catch (NullReferenceException e) 
                    {
                        MessageBox.Show("S52 file format error.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public ColorTableViewModel ColorTable
        {
            get { return colorTableViewModel; }
            set
            {
                if (colorTableViewModel != value)
                {
                    colorTableViewModel = value;
                    RaisePropertyChanged("ColorTable");
                }
            }
        }

        public SymbolTableViewModel SymbolTable
        {
            get { return symbolTableViewModel; }
            set
            {
                if (symbolTableViewModel != value)
                {
                    symbolTableViewModel = value;
                    RaisePropertyChanged("SymbolTable");
                }
            }
        }


        public LookupTableViewModel LookupTable
        {
            get { return lookupTableViewModel; }
            set
            {
                if (lookupTableViewModel != value)
                {
                    lookupTableViewModel = value;
                    RaisePropertyChanged("LookupTable");
                }
            }
        }

        public ICommand S52ResourceBrowerCommand
        {
            get
            {
                return s52ResourceBrowerCommand ?? (s52ResourceBrowerCommand = new RelayCommand(HandleS52ResourceBrowerCommand));
            }
        }

        private void HandleS52ResourceBrowerCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog() 
            {
                Filter = "(*.xml)|*.xml"
            };
            if (dialog.ShowDialog() ?? false)
            {
                S52SymbolsSourcePath = dialog.FileName;
            }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand ?? (cancelCommand = new RelayCommand(HandleCancelCommand)); }
        }

        private void HandleCancelCommand()
        {
            Messenger.Default.Send<WindowStateMessage>(new WindowStateMessage(S57WindowState.Close));
        }
    }
}
