using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace NauticalChartsViewer
{

    public partial class SymbolsCreatingWindow : Window
    {
        private SymbolsCreatingViewModel symbolsCreatingViewModel;

        public SymbolsCreatingWindow()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                Messenger.Default.Register<WindowStateMessage>(this, "SymbolsCreatingWindow", (u) =>
                {
                    if (u.WindowState == S57WindowState.Close)
                    {
                        Close();
                    }
                });
            };
            Unloaded += (sender, e) =>
            {
                Messenger.Default.Unregister(this);
                symbolsCreatingViewModel.Cleanup();
            };
            symbolsCreatingViewModel = new SymbolsCreatingViewModel();
            DataContext = symbolsCreatingViewModel;
        }
    }
}
