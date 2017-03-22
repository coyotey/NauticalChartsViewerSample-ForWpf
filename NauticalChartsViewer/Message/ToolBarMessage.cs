
using GalaSoft.MvvmLight.Messaging;
namespace NauticalChartsViewer
{
    public class ToolBarMessage : MessageBase
    {
        public ToolBarMessage(string action) 
        {
            Action = action;
        }
        public string Action { get; private set; }
    }
}
