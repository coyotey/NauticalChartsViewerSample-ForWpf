using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ThinkGeo.MapSuite
{
    public class ChartTable
    {
        private string name;
        private Dictionary<string, ChartStyleModule> chartStyleModules;

        public ChartTable()
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Dictionary<string, ChartStyleModule> ChartStyleModules
        {
            get
            {
                if (chartStyleModules == null)
                {
                    chartStyleModules = new Dictionary<string, ChartStyleModule>();
                }
                return chartStyleModules;
            }
        }

        internal ChartStyleModule ChartStyleModule
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

       
        internal static Dictionary<string, ChartTable> Read(XmlDocument doc)
        {
            Dictionary<string, ChartTable> chartTables = new Dictionary<string, ChartTable>();

            // select "charts" node
            XmlNode chartsNode = doc.DocumentElement.SelectSingleNode("charts");
            foreach (XmlNode chartTablesNode in chartsNode.ChildNodes)
            {
                ChartTable chartTable = new ChartTable();
                chartTable.Name = chartTablesNode.Name;

                foreach (XmlNode chartTableNode in chartTablesNode.ChildNodes)
                {
                    ChartStyleModule styleModule = null;
                    switch (chartTable.Name)
                    {
                        case "symbols":
                            styleModule = new SymbolStyleModule()
                            {
                                CommandType = chartTableNode.Attributes["type"].Value[0]
                            };
                            break;

                        case "lines":
                            styleModule = new LineStyleModule();
                            break;

                        case "patterns":
                            styleModule = new PatternStyleModule()
                            {
                                CommandType = chartTableNode.Attributes["type"].Value[0],
                                FillType = chartTableNode.SelectSingleNode("fillType").InnerText,
                                SymbolSpacingType = chartTableNode.SelectSingleNode("spacingType").InnerText,
                                MinDistance = Convert.ToInt32(chartTableNode.SelectSingleNode("minimumDistance").InnerText, CultureInfo.InvariantCulture),
                                MaxDistance = Convert.ToInt32(chartTableNode.SelectSingleNode("maximumDistance").InnerText, CultureInfo.InvariantCulture)
                            };
                            break;

                        default:
                            break;
                    }

                    styleModule.ModuleName = chartTableNode.SelectSingleNode("module").InnerText;
                    styleModule.Rcid = Convert.ToInt32(chartTableNode.Attributes["rcid"].Value, CultureInfo.InvariantCulture);
                    styleModule.Name = chartTableNode.Attributes["name"].Value;

                    // Information related to geometry
                    XmlNode geoNode = chartTableNode.SelectSingleNode("geometry");
                    styleModule.BoundingBox = new Size(Convert.ToInt32(geoNode.Attributes["width"].Value, CultureInfo.InvariantCulture), Convert.ToInt32(geoNode.Attributes["height"].Value, CultureInfo.InvariantCulture));
                    styleModule.Pivot = new Point(Convert.ToInt32(geoNode.SelectSingleNode("pivotX").InnerText, CultureInfo.InvariantCulture), Convert.ToInt32(geoNode.SelectSingleNode("pivotY").InnerText, CultureInfo.InvariantCulture));
                    styleModule.UpperLeft = new Point(Convert.ToInt32(geoNode.SelectSingleNode("upperLeftX").InnerText, CultureInfo.InvariantCulture), Convert.ToInt32(geoNode.SelectSingleNode("upperLeftY").InnerText, CultureInfo.InvariantCulture));

                    XmlNode colorRefsNode = chartTableNode.SelectSingleNode("colorRefs");
                    if (colorRefsNode != null)
                    {
                        foreach (XmlNode colorRefNode in colorRefsNode.ChildNodes)
                        {
                            styleModule.ColorReferences.Add(colorRefNode.Attributes["index"].Value[0], colorRefNode.Attributes["token"].Value);
                        }
                    }

                    XmlNode commandsNode = chartTableNode.SelectSingleNode("commands");
                    if (commandsNode != null)
                    {
                        foreach (XmlNode commandNode in commandsNode.ChildNodes)
                        {
                            styleModule.Commands.Add(commandNode.InnerText);
                        }
                    }

                    chartTable.ChartStyleModules.Add(styleModule.Name, styleModule);
                }

                chartTables.Add(chartTable.Name, chartTable);
            }

            return chartTables;
        }
    }
}