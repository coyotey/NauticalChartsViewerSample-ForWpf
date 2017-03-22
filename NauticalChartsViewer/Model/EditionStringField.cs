
namespace NauticalChartsViewer
{
    public class EditionStringField : ObservableObject
    {
        private string stringField;
        public EditionStringField(string stringField)
        {
            this.stringField = stringField;
        }
        public string StringField
        {
            get { return stringField; }
            set
            {
                if (stringField != value)
                {
                    stringField = value;
                    RaisePropertyChanged("StringField");
                }
            }
        }
    }
}
