
namespace ThinkGeo.MapSuite
{
    internal abstract class S52BaseSymbol
    {
        private string symbolValue;

        public S52BaseSymbol()
            : this(string.Empty)
        {

        }

        public S52BaseSymbol(string value)
        {
            this.symbolValue = value;
        }

        public string Value
        {
            get { return symbolValue; }
            set { symbolValue = value; }
        }

        public string GetHeader()
        {
            if (string.IsNullOrEmpty(symbolValue))
            {
                return string.Empty;
            }
            else
            {
                return symbolValue.Substring(0, 2);
            }
        }

        public string GetBody()
        {
            if (string.IsNullOrEmpty(symbolValue))
            {
                return string.Empty;
            }
            else
            {
                return symbolValue.Substring(2);
            }
        }
    }
}
