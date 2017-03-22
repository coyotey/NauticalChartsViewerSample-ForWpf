using System;

namespace ThinkGeo.MapSuite
{
    internal class STSymbol : S52BaseSymbol
    {
        public STSymbol()
            : this(string.Empty)
        {
            
        }

        public STSymbol(string value)
            : base(value)
        {
            
        }

        public int GetTransparency()
        {
            if (string.IsNullOrEmpty(Value))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(GetBody());
            }
        }
    }
}
