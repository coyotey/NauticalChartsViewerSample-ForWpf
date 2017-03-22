using System;

namespace ThinkGeo.MapSuite
{
    internal class CISymbol : S52BaseSymbol
    {
        public CISymbol()
            : this(string.Empty)
        {
            
        }

        public CISymbol(string value)
            : base(value)
        {
            
        }

        public int GetRadius()
        {
            return Convert.ToInt32(GetBody());
        }
    }
}
