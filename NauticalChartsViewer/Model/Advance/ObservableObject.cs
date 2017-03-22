using System.ComponentModel;

namespace NauticalChartsViewer
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName) 
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) 
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
