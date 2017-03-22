using System;
using ThinkGeo.MapSuite.Drawing;

namespace ThinkGeo.MapSuite
{
    public class ColorEntry
    {
        private string token;
        private string name;
        private GeoColor color;

        public ColorEntry()
        { }

        public ColorEntry(string token, Int16 red, Int16 green, Int16 blue)
            : this(token, string.Empty, red, green, blue)
        {
        }

        public ColorEntry(string token, string name, Int16 red, Int16 green, Int16 blue)
        {
            this.Token = token;
            this.Name = name;
            this.Color = GeoColor.FromArgb(255, red, green, blue);
        }

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public GeoColor Color
        {
            get { return color; }
            set { color = value; }
        }

    }
}