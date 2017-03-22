using GalaSoft.MvvmLight.Messaging;

namespace NauticalChartsViewer
{
    internal class ChartSelectedItemMessage : MessageBase
    {
        private ChartSelectedItem chartSelectedItem;

        public ChartSelectedItem ChartSelectedItem
        {
            get
            {
                return chartSelectedItem;
            }
        }

        public ChartSelectedItemMessage(ChartSelectedItem selectedItem)
        {
            chartSelectedItem = selectedItem;
        }
    }
}
