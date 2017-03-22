using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Xml;

namespace NauticalChartsViewer
{
    public class MenuItemHelper
    {
        public static Collection<object> GetMenus()
        {
            XmlDocument doc = new XmlDocument();
            Stream stream = App.GetResourceStream(new Uri("pack://application:,,,/resource/menus.xml")).Stream;
            doc.Load(stream);

            XmlNode root = doc.SelectSingleNode("menuSet");
            XmlNodeList menuNodes = root.ChildNodes;

            CompositeMenuItem compositeMenuItem = new CompositeMenuItem("root", new Collection<object>());

            foreach (XmlNode menu in menuNodes)
            {
                Parse(menu, compositeMenuItem);

            }
            return compositeMenuItem.Children;
        }

        private static void Parse(XmlNode xmlNode, CompositeMenuItem menu)
        {
            switch (xmlNode.Name.ToLower())
            {
                case "menu":
                    {
                        CompositeMenuItem compositeMenuItem = new CompositeMenuItem(xmlNode.Attributes["header"].Value, new Collection<object>());
                        compositeMenuItem.Parent = menu;
                        XmlNode node = xmlNode.SelectSingleNode("menuItems");
                        Parse(node, compositeMenuItem);
                        menu.Children.Add(compositeMenuItem);
                    }
                    break;
                case "menuitems":
                    {
                        XmlNodeList childNodes = xmlNode.ChildNodes;
                        foreach (XmlNode node in childNodes)
                        {
                            Parse(node, menu);
                        }
                    }
                    break;
                case "menuitem":
                    {
                        XmlAttributeCollection attrs = xmlNode.Attributes;
                        bool isCheckEnabled = attrs["isCheckable"] != null ? Convert.ToBoolean(attrs["isCheckable"].Value) : false;
                        bool isChecked = attrs["isChecked"] != null ? Convert.ToBoolean(attrs["isChecked"].Value) : false;
                        bool canToggle = attrs["canToggle"] != null ? Convert.ToBoolean(attrs["canToggle"].Value) : false;
                        string icon = attrs["icon"] != null ? attrs["icon"].Value : string.Empty;
                        menu.Children.Add(new SingleMenuItem(icon, attrs["header"].Value, attrs["action"].Value, attrs["groupName"].Value, isCheckEnabled, isChecked, canToggle, menu));
                    }
                    break;
                case "separator":
                    {
                        menu.Children.Add(new Separator());
                    }
                    break;
                default:
                    break; ;
            }
        }
    }
}