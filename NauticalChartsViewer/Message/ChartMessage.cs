using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace NauticalChartsViewer
{
    internal class ChartMessage : MessageBase
    {
        public ChartMessage() { }

        public ChartMessage(IEnumerable<ChartItem> charts)
        {
            Charts = charts;
        }

        public IEnumerable<ChartItem> Charts { get; set; }

    }
}
