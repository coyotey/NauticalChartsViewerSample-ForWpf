using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ThinkGeo.MapSuite
{
    public class LookupTable
    {
        private LookupTableType drawnType;
        private Collection<LookupTableModule> lookupModules;

        public LookupTable()
        {
        }

        public LookupTableType DrawnType
        {
            get { return drawnType; }
            set { drawnType = value; }
        }

        public Collection<LookupTableModule> LookupModules
        {
            get
            {
                if (lookupModules == null)
                {
                    lookupModules = new Collection<LookupTableModule>();
                }

                return lookupModules;
            }
        }

        internal LookupTableModule LookupTableModule
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


        internal static Dictionary<LookupTableType, LookupTable> Read(XmlDocument doc)
        {
            // select "lookups" node
            Dictionary<LookupTableType, LookupTable> lookupTables = new Dictionary<LookupTableType, LookupTable>();

            XmlNode lookupsNode = doc.DocumentElement.SelectSingleNode("lookups");
            foreach (XmlNode lookupTableNode in lookupsNode.ChildNodes)
            {
                LookupTable lookupTable = new LookupTable()
                {
                    DrawnType = GetLookTableType(lookupTableNode.Attributes["name"].InnerText)
                };
                string shapeType = lookupTableNode.Attributes["shapeType"].InnerText;

                XmlNodeList lookups = lookupTableNode.SelectNodes("lookup");
                foreach (XmlNode lookup in lookups)
                {
                    LookupTableModule lookupModule = new LookupTableModule()
                    {
                        Rcid = Convert.ToInt32(lookup.Attributes["rcid"].InnerText, CultureInfo.InvariantCulture),
                        ModuleName = lookup.SelectSingleNode("module").InnerText,
                        ObjectClass = lookup.SelectSingleNode("objectClass").InnerText,
                        DisplayType = shapeType,
                        DisplayPriority = Convert.ToInt32(lookup.SelectSingleNode("displayPriority").InnerText, CultureInfo.InvariantCulture),
                        RadarPriority = lookup.SelectSingleNode("radarPriority").InnerText[0],
                        DisplayCategory = lookup.Attributes["category"].InnerText
                    };

                    XmlNode attributesNode = lookup.SelectSingleNode("attributes");
                    if (attributesNode != null)
                    {
                        foreach (XmlNode attrNode in attributesNode.ChildNodes)
                        {
                            lookupModule.Attributes.Add(attrNode.Name, attrNode.InnerText);
                        }
                    }

                    XmlNode instructionsNode = lookup.SelectSingleNode("instructions");
                    if (instructionsNode != null)
                    {
                        foreach (XmlNode instrNode in instructionsNode.ChildNodes)
                        {
                            lookupModule.Instructions.Add(instrNode.InnerText);
                        }
                    }
                    lookupTable.LookupModules.Add(lookupModule);
                }

                lookupTables.Add(GetLookTableType(lookupTableNode.Attributes["name"].InnerText), lookupTable);
            }

            return lookupTables;
        }

        public static LookupTableType GetLookTableType(string lookupName)
        {
            LookupTableType showStyle = LookupTableType.UNKNOWN;
            switch (lookupName)
            {
                case "LINES":
                    showStyle = LookupTableType.LINES;
                    break;
                case "PAPER_CHART":
                    showStyle = LookupTableType.PAPER_CHART;
                    break;
                case "PLAIN_BOUNDARIES":
                    showStyle = LookupTableType.PLAIN_BOUNDARIES;
                    break;
                case "SIMPLIFIED":
                    showStyle = LookupTableType.SIMPLIFIED;
                    break;
                case "SYMBOLIZED_BOUNDARIES":
                    showStyle = LookupTableType.SYMBOLIZED_BOUNDARIES;
                    break;
                default:
                    break;
            }

            return showStyle;
        }

    }
}