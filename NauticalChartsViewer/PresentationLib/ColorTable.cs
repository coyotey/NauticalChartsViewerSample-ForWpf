using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using ThinkGeo.MapSuite.Layers;

namespace ThinkGeo.MapSuite
{
    public class ColorTable : LibraryFormatModule
    {
        private string name;
        private Collection<ColorEntry> colorEntries;

        public ColorTable()
        { }

        public ColorTable(string name)
            : this(name, new Collection<ColorEntry>())
        { }

        public ColorTable(string name, IList<ColorEntry> colorEntries)
            : base()
        {
            this.Name = name;
            this.colorEntries = new Collection<ColorEntry>(colorEntries);
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Collection<ColorEntry> ColorEntries
        {
            get
            {
                if (colorEntries == null)
                {
                    colorEntries = new Collection<ColorEntry>();
                }

                return colorEntries;
            }
            set { colorEntries = value; }
        }

        internal static Dictionary<NauticalChartsDefaultColorSchema, ColorTable> Read(XmlDocument doc)
        {
            Dictionary<NauticalChartsDefaultColorSchema, ColorTable> colorTables = new Dictionary<NauticalChartsDefaultColorSchema, ColorTable>();
            XmlNode colorsNode = doc.DocumentElement.SelectSingleNode(@"ColorSchemas");
            foreach (XmlNode colorTableNode in colorsNode.ChildNodes)
            {
                ColorTable colorTable = new ColorTable(colorTableNode.Attributes["name"].Value);
                NauticalChartsDefaultColorSchema colorSchema = (NauticalChartsDefaultColorSchema)Enum.Parse(typeof(NauticalChartsDefaultColorSchema), colorTableNode.Attributes["name"].Value, true);

                Collection<ColorEntry> colorEntries = new Collection<ColorEntry>();
                foreach (XmlNode colorEntryNode in colorTableNode.ChildNodes)
                {
                    ColorEntry colorEntry = new ColorEntry(colorEntryNode.Attributes["token"].Value, short.Parse(colorEntryNode.Attributes["r"].Value), short.Parse(colorEntryNode.Attributes["g"].Value), short.Parse(colorEntryNode.Attributes["b"].Value));
                    colorEntries.Add(colorEntry);
                }

                colorTable.ColorEntries = colorEntries;
                colorTables.Add(colorSchema, colorTable);
            }

            return colorTables;
        }

    }
}