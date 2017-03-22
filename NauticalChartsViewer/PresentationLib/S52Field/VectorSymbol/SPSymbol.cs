using System.Collections.Generic;

namespace ThinkGeo.MapSuite
{
    internal class SPSymbol : S52BaseSymbol
    {
        public SPSymbol()
            : this(string.Empty)
        {
        }

        public SPSymbol(string value)
            : base(value)
        {
        }

        public RGBColor GetColor(Dictionary<string, RGBColor> colorRef)
        {
            string body = GetBody();
            if (string.IsNullOrEmpty(body))
            {
                return new RGBColor();
            }
            else
            {
                return colorRef[body];
            }
        }
    }
}