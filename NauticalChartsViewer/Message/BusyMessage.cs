using GalaSoft.MvvmLight.Messaging;

namespace NauticalChartsViewer
{
    public class BusyMessage : MessageBase
    {
        public BusyMessage() { }

        public BusyMessage(bool isBusy)
        {
            IsBusy = isBusy;
        }

        public bool IsBusy { get; set; }
    }
}
