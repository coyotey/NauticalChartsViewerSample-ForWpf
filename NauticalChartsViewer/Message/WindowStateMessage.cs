using GalaSoft.MvvmLight.Messaging;

namespace NauticalChartsViewer
{
    public class WindowStateMessage : MessageBase
    {
        public WindowStateMessage() { }

        public WindowStateMessage(S57WindowState state)
        {
            WindowState = state;
        }

        public S57WindowState WindowState { get; set; }


    }
}
