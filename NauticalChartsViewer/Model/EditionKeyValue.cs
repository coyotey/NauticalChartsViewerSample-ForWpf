
namespace NauticalChartsViewer
{
    public class EditionKeyValue : ObservableObject
    {
        private string key;
        private string value;

        public EditionKeyValue(string key, string value)
        {
            this.value = value;
            this.key = key;
        }
        public string Key
        {
            get { return key; }
        }

        public string Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }
    }
}
