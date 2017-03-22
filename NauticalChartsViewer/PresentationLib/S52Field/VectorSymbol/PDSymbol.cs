using System;
using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class PDSymbol : S52BaseSymbol
    {
        public PDSymbol()
            : this(string.Empty)
        {
            
        }

        public PDSymbol(string value)
            : base(value)
        {
            
        }

        public Collection<Vertex> GetVertexes()
        {
            Collection<Vertex> vertexes = new Collection<Vertex>();
            string body = GetBody();
            if (string.IsNullOrEmpty(body))
            {
                return vertexes;
            }
            else
            {
                string[] bodyPart = body.Split(',');
                for (int i = 0; i < bodyPart.Length; i += 2)
                {
                    int x = Convert.ToInt32(bodyPart[i]);
                    int y = Convert.ToInt32(bodyPart[i + 1]);
                    vertexes.Add(new Vertex(x, y));
                }
                return vertexes;
            }
        }
    }
}
