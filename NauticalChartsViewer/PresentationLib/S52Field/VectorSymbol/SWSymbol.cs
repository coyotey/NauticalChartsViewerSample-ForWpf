using System;

namespace ThinkGeo.MapSuite
{
    internal class SWSymbol : S52BaseSymbol
    {
        public SWSymbol()
            : this(string.Empty)
        {
            
        }

        public SWSymbol(string value)
            : base(value)
        {
            
        }

        public int GetWidth()
        {
            string body = GetBody();
            if (string.IsNullOrEmpty(body))
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(GetBody());
            }
        }
    }
}
