using System;

namespace ThinkGeo.MapSuite
{
    internal class AASymbol : S52BaseSymbol
    {
        public AASymbol()
            : this(string.Empty)
        {
            
        }

        public AASymbol(string value)
            : base(value)
        {
            
        }

        public Vertex GetCenterVertex()
        {
            string[] bodyPart = GetBody().Split(',');
            int x = Convert.ToInt32(bodyPart[0]);
            int y = Convert.ToInt32(bodyPart[1]);
            return new Vertex(x, y);
        }

        public int GetAngle()
        {
            string[] bodyPart = GetBody().Split(',');
            return Convert.ToInt32(bodyPart[2]);
        }
    }
}
