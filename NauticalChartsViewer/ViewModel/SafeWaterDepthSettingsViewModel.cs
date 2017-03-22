using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using ThinkGeo.MapSuite.Layers;

namespace NauticalChartsViewer
{

    public class SafeWaterDepthSettingsViewModel : ViewModelBase
    {
        private double safeWaterDepth = Globals.SafetyDepth;
        private double shallowWaterDepth = Globals.ShallowDepth;
        private double deepWaterDepth = Globals.DeepDepth;
        private double safeContourDepth = Globals.SafetyContour;
        private NauticalChartsDepthUnit depthUnit = Globals.CurrentDepthUnit;
        private ICommand optionCommand;
       
        public SafeWaterDepthSettingsViewModel()
        {
        }

        public double SafetyWaterDepth
        {
            get { return safeWaterDepth; }
            set { safeWaterDepth = value; }
        }

        public double SafetyContourDepth
        {
            get { return safeContourDepth; }
            set { safeContourDepth = value; }
        }

        public double ShallowWaterDepth
        {
            get { return shallowWaterDepth; }
            set { shallowWaterDepth = value; }
        }
        public double DeepWaterDepth
        {
            get { return deepWaterDepth; }
            set { deepWaterDepth = value; }
        }

        public NauticalChartsDepthUnit DepthUnit
        {
            get { return depthUnit; }
            set { depthUnit = value; }
        }

        public ICommand OptionCommand
        {
            get { return optionCommand ?? (optionCommand = new RelayCommand<object>(HandleOptionCommand)); }
        }


        private void HandleOptionCommand(object option)
        {
            var parameterOption = option as string;
            if (parameterOption == "OK")
            {
                SafeWaterDepthSettingMessage safeWaterMessage = new SafeWaterDepthSettingMessage(SafetyContourDepth, SafetyWaterDepth, ShallowWaterDepth, DeepWaterDepth, DepthUnit);
                Messenger.Default.Send(safeWaterMessage);
            }

            WindowStateMessage windowMessage = new WindowStateMessage(S57WindowState.Close);
            Messenger.Default.Send(windowMessage, "SafeWaterDepthSettingsWindow");
        }

        //private void HandleMenuCommand(object item)
        //{
        //    var menuItem = item as BaseMenuItem;
        //    if (menuItem != null)
        //    {
        //        if (!IsInvalidOperation(menuItem))
        //        {
        //            SwitchCheckedState(menuItem, menuItems);
        //            var message = new MenuItemMessage(menuItem);
        //            Messenger.Default.Send<MenuItemMessage>(message);
        //        }
        //        else
        //        {
        //            menuItem.IsChecked = true;
        //        }
        //    }
        //}
    }
}