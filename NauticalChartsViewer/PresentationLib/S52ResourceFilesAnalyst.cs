using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ThinkGeo.MapSuite.Layers;

namespace ThinkGeo.MapSuite
{
    public class S52ResourceFilesAnalyst
    {
        private string resourceFile;
        private Dictionary<NauticalChartsDefaultColorSchema, ColorTable> colorSchemas;
        private Dictionary<LookupTableType, LookupTable> lookupTables;
        private Dictionary<string, ChartTable> chartTables;


        private S52ResourceFilesAnalyst()
        { }

        public S52ResourceFilesAnalyst(string resourceFile)
        {
            this.resourceFile = resourceFile;

            XmlDocument doc = new XmlDocument();
            doc.Load(resourceFile);

            this.colorSchemas = ColorTable.Read(doc);

            this.lookupTables = LookupTable.Read(doc);

            this.chartTables = ChartTable.Read(doc);
        }

        public Dictionary<SymbolType, Collection<ChartStyleModule>> GetSymbolModules()
        {
            Dictionary<SymbolType, Collection<ChartStyleModule>> symbols = new Dictionary<SymbolType, Collection<ChartStyleModule>>();

            IEnumerable<ChartStyleModule> symbolsCollection = chartTables.Where(x => x.Key == "symbols").SelectMany(x => x.Value.ChartStyleModules.Values).Cast<ChartStyleModule>();
            IEnumerable<ChartStyleModule> linesCollection = chartTables.Where(x => x.Key == "lines").SelectMany(x => x.Value.ChartStyleModules.Values).Cast<ChartStyleModule>();
            IEnumerable<ChartStyleModule> patternsCollection = chartTables.Where(x => x.Key == "patterns").SelectMany(x => x.Value.ChartStyleModules.Values).Cast<ChartStyleModule>();

            symbols.Add(SymbolType.Symbols, new Collection<ChartStyleModule>(symbolsCollection.ToList()));
            symbols.Add(SymbolType.Lines, new Collection<ChartStyleModule>(linesCollection.ToList()));
            symbols.Add(SymbolType.Patterns, new Collection<ChartStyleModule>(patternsCollection.ToList()));

            return symbols;
        }

        public SymbolStyleModule GetSymbolModuleByName(SymbolType symbolType, string name)
        {
            string type = "symbols";
            switch (symbolType)
            {
                case SymbolType.Symbols:
                    break;
                case SymbolType.Lines:
                    type = "lines";
                    break;
                case SymbolType.Patterns:
                    type = "patterns";
                    break;
                default:
                    break;
            }

            IEnumerable<SymbolStyleModule> chartTablesList = chartTables.Where(x => x.Key == type && x.Value.Name == name).Select(x => x.Value.ChartStyleModules.Values).Cast<SymbolStyleModule>();
            SymbolStyleModule chartTable = chartTablesList.First();
            return chartTable; // Todo: please implement it
        }

        public void UpdateChartTableEntry(SymbolType symbolType, ChartStyleModule newChartTableModule)
        {
            switch (symbolType)
            {
                case SymbolType.Symbols:
                    UpdateSymbolTableEntry(newChartTableModule);
                    break;
                case SymbolType.Lines:
                    UpdateLineTableEntry(newChartTableModule);
                    break;
                case SymbolType.Patterns:
                    UpdatePatternTableEntry(newChartTableModule);
                    break;
            }
        }

        private void UpdateSymbolTableEntry(ChartStyleModule chartStyleModule)
        {
            SymbolStyleModule module = chartStyleModule as SymbolStyleModule;
            XDocument document = XDocument.Load(resourceFile);
            XElement element = document.Root.XPathSelectElements("charts/symbols/symbol").Where(x => x.Attribute("rcid").Value == module.Rcid.ToString()).First();
            if (element != null)
            {
                element.SetElementValue("module", module.ModuleName);
                element.SetElementValue("pivotX", module.Pivot.X.ToString());
                element.SetElementValue("pivotY", module.Pivot.Y.ToString());
                element.SetElementValue("upperLeftX", module.UpperLeft.X.ToString());
                element.SetElementValue("upperLeftY", module.UpperLeft.Y.ToString());
                element.XPathSelectElements("colorRefs/colorRef").Remove();
                XElement colorRefsElement = element.Element("colorRefs");
                foreach (var item in module.ColorReferences)
                {
                    XElement xElement = new XElement("colorRef");
                    XAttribute indexAttr = new XAttribute("index", item.Key);
                    XAttribute tokenAttr = new XAttribute("token", item.Value);
                    xElement.Add(indexAttr);
                    xElement.Add(tokenAttr);
                    colorRefsElement.Add(xElement);
                }

                element.XPathSelectElements("commands/command").Remove();
                XElement commandsElement = element.Element("commands");
                foreach (var item in module.Commands)
                {
                    XElement commandElement = new XElement("command", item);
                    commandsElement.Add(commandElement);
                }

                document.Save(resourceFile);
            }
        }

        private void UpdateLineTableEntry(ChartStyleModule chartStyleModule)
        {
            LineStyleModule module = chartStyleModule as LineStyleModule;
            XDocument document = XDocument.Load(resourceFile);
            XElement element = document.Root.XPathSelectElements("charts/lines/line").Where(x => x.Attribute("rcid").Value.ToString() == module.Rcid.ToString()).First();
            if (element != null)
            {
                element.SetElementValue("module", module.ModuleName);
                element.SetElementValue("pivotX", module.Pivot.X.ToString());
                element.SetElementValue("pivotY", module.Pivot.Y.ToString());
                element.SetElementValue("upperLeftX", module.UpperLeft.X.ToString());
                element.SetElementValue("upperLeftY", module.UpperLeft.Y.ToString());
                element.XPathSelectElements("colorRefs/colorRef").Remove();
                XElement colorRefsElement = element.Element("colorRefs");
                foreach (var item in module.ColorReferences)
                {
                    XElement xElement = new XElement("colorRef");
                    XAttribute indexAttr = new XAttribute("index", item.Key);
                    XAttribute tokenAttr = new XAttribute("token", item.Value);
                    xElement.Add(indexAttr);
                    xElement.Add(tokenAttr);
                    colorRefsElement.Add(xElement);
                }

                element.XPathSelectElements("commands/command").Remove();
                XElement commandsElement = element.Element("commands");
                foreach (var item in module.Commands)
                {
                    XElement commandElement = new XElement("command", item);
                    commandsElement.Add(commandElement);
                }

                document.Save(resourceFile);
            }
        }

        private void UpdatePatternTableEntry(ChartStyleModule chartStyleModule)
        {
            PatternStyleModule module = chartStyleModule as PatternStyleModule;
            XDocument document = XDocument.Load(resourceFile);
            XElement element = document.Root.XPathSelectElements("charts/patterns/patternSymbol").Where(x => x.Attribute("rcid").Value.ToString() == module.Rcid.ToString()).First();
            if (element != null)
            {
                element.SetElementValue("module", module.ModuleName);
                element.XPathSelectElement("geometry/pivotX").SetValue(module.Pivot.X.ToString());
                element.XPathSelectElement("geometry/pivotY").SetValue(module.Pivot.Y.ToString());
                element.XPathSelectElement("geometry/upperLeftX").SetValue(module.UpperLeft.X.ToString());
                element.XPathSelectElement("geometry/upperLeftY").SetValue(module.UpperLeft.Y.ToString());
                element.XPathSelectElements("colorRefs/colorRef").Remove();
                XElement colorRefsElement = element.Element("colorRefs");
                foreach (var item in module.ColorReferences)
                {
                    XElement xElement = new XElement("colorRef");
                    XAttribute indexAttr = new XAttribute("index", item.Key);
                    XAttribute tokenAttr = new XAttribute("token", item.Value);
                    xElement.Add(indexAttr);
                    xElement.Add(tokenAttr);
                    colorRefsElement.Add(xElement);
                }

                element.XPathSelectElements("commands/command").Remove();
                XElement commandsElement = element.Element("commands");
                foreach (var item in module.Commands)
                {
                    XElement commandElement = new XElement("command", item);
                    commandsElement.Add(commandElement);
                }

                element.Element("fillType").SetValue(module.FillType);
                element.Element("spacingType").SetValue(module.SymbolSpacingType);
                element.Element("minimumDistance").SetValue(module.MinDistance.ToString());
                element.Element("maximumDistance").SetValue(module.MaxDistance.ToString());
                document.Save(resourceFile);
            }
        }

        public Dictionary<LookupTableType, Collection<LookupTableModule>> GetLookupEntries()
        {
            Dictionary<LookupTableType, Collection<LookupTableModule>> lookupEntries = new Dictionary<LookupTableType, Collection<LookupTableModule>>();
            foreach (var lookupTable in this.lookupTables)
            {
                Collection<LookupTableModule> modules = new Collection<LookupTableModule>();
                foreach (var module in lookupTable.Value.LookupModules)
                {
                    modules.Add(module);
                }

                lookupEntries.Add(lookupTable.Key, modules);
            }

            return lookupEntries;
        }

        public LookupTableModule GetLookTableModuleByName(LookupTableType lookupTableType, string name)
        {
            LookupTable lookupTable = lookupTables[lookupTableType];

            return lookupTable.LookupModules.Where(entry => entry.ModuleName.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public void UpdateLookupTableEntry(LookupTableType lookupTableType, LookupTableModule newLookupTableModule)
        {
            switch (lookupTableType)
            {
                case LookupTableType.LINES:
                    UpdateLookupTableEntry("Lines", newLookupTableModule);
                    break;
                case LookupTableType.PAPER_CHART:
                    UpdateLookupTableEntry("PaperChart", newLookupTableModule);
                    break;
                case LookupTableType.PLAIN_BOUNDARIES:
                    UpdateLookupTableEntry("PlainBoundaries", newLookupTableModule);
                    break;
                case LookupTableType.SIMPLIFIED:
                    UpdateLookupTableEntry("Simplified", newLookupTableModule);
                    break;
                case LookupTableType.SYMBOLIZED_BOUNDARIES:
                    UpdateLookupTableEntry("SymbolizedBoundaries", newLookupTableModule);
                    break;
            }
        }

        private void UpdateLookupTableEntry(string type, LookupTableModule newLookupTableModule)
        {
            XDocument document = XDocument.Load(resourceFile);
            XElement element = document.Root.XPathSelectElements("lookups/" + type + "/lookup").Where(x => x.Attribute("rcid").Value.ToString() == newLookupTableModule.Rcid.ToString()).First();
            if (element != null)
            {
                element.Attribute("category").SetValue(newLookupTableModule.DisplayCategory);
                element.Element("objectClass").SetValue(newLookupTableModule.ObjectClass);
                element.Element("displayPriority").SetValue(newLookupTableModule.DisplayPriority.ToString());
                element.Element("radarPriority").SetValue(newLookupTableModule.RadarPriority.ToString());
                element.Element("module").SetValue(newLookupTableModule.ModuleName);
                element.XPathSelectElements("instructions/instr").Remove();
                XElement instructionsElement = element.Element("instructions");
                foreach (var item in newLookupTableModule.Instructions)
                {
                    XElement xElement = new XElement("instr");
                    xElement.SetValue(item);
                    instructionsElement.Add(xElement);
                }

                XElement attributesElement = element.Element("attributes");
                if (newLookupTableModule.Attributes.Count > 0)
                {
                    if (attributesElement == null)
                    {
                        element.Add(new XElement("attributes"));
                    }
                    else
                    {
                        attributesElement.Elements().Remove();
                    }

                    attributesElement = element.Element("attributes");

                    foreach (var item in newLookupTableModule.Attributes)
                    {
                        attributesElement.Add(new XElement(item.Key, item.Value));
                    }
                }
                document.Save(resourceFile);
            }
        }

        public Dictionary<NauticalChartsDefaultColorSchema, Collection<ColorEntry>> GetColorEntries()
        {
            Dictionary<NauticalChartsDefaultColorSchema, Collection<ColorEntry>> colorEntries = new Dictionary<NauticalChartsDefaultColorSchema, Collection<ColorEntry>>();
            foreach (var colorSchema in colorSchemas)
            {
                Collection<ColorEntry> tempColorEntries = new Collection<ColorEntry>();
                foreach (var colorEntry in colorSchema.Value.ColorEntries)
                {
                    tempColorEntries.Add(colorEntry);
                }
                colorEntries.Add(colorSchema.Key, tempColorEntries);
            }

            return colorEntries;
        }

        public ColorEntry GetColorEntryByToken(NauticalChartsDefaultColorSchema colorSchema, string colorToken)
        {
            ColorTable colorTable = this.colorSchemas[colorSchema];

            return colorTable.ColorEntries.Where(entry => entry.Token.Equals(colorToken, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public void UpdateColorEntry(NauticalChartsDefaultColorSchema colorSchema, ColorEntry newColorEntry)
        {
            XDocument document = XDocument.Load(resourceFile);
            XElement element = document.Root.XPathSelectElements("ColorSchemas/" + colorSchema.ToString().ToUpperInvariant() + "/Color").Where(x => x.Attribute("token").Value.ToString() == newColorEntry.Token).First();
            if (element != null)
            {
                element.Attribute("r").SetValue(newColorEntry.Color.RedComponent);
                element.Attribute("g").SetValue(newColorEntry.Color.GreenComponent);
                element.Attribute("b").SetValue(newColorEntry.Color.BlueComponent);
                document.Save(resourceFile);
            }
        }
    }
}
