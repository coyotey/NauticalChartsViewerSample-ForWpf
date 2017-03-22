using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace NauticalChartsViewer
{
    public class SymbolItem : ICloneable
    {
        private Size boundingBox;
        private Dictionary<char, string> colorReferences;
        private Collection<string> commands;
        private Collection<EditionStringField> editionCommands;
        private char commandType;
        private string name;
        private Point pivot;
        private Point upperLeft;
        private int rcid;

        public SymbolItem(int rcid, Size boundingBox, Dictionary<char, string> colorReferences, Collection<string> commands, char
            commandType, string name, Point pivot, Point upperLeft)
        {
            this.rcid = rcid;
            this.boundingBox = boundingBox;
            this.colorReferences = colorReferences;
            this.commands = commands;
            this.commandType = commandType;
            this.name = name;
            this.pivot = pivot;
            this.upperLeft = upperLeft;
        }

        public int Rcid
        {
            get { return rcid; }
            set { rcid = value; }
        }

        public Size BoundingBox
        {
            get
            {
                return boundingBox;
            }
            set
            {
                boundingBox = value;
            }
        }

        public Dictionary<char, string> ColorReferences
        {
            get
            {
                if (colorReferences == null)
                {
                    colorReferences = new Dictionary<char, string>();
                }

                return colorReferences;
            }
        }

        public Collection<EditionStringField> EditionCommands
        {
            get
            {
                if (editionCommands == null)
                {
                    editionCommands = new Collection<EditionStringField>();
                    foreach (string command in commands)
                    {
                        editionCommands.Add(new EditionStringField(command));
                    }
                }
                return editionCommands;
            }
        }

        public Collection<string> Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new Collection<string>();
                }

                return commands;
            }
        }

        public char CommandType
        {
            get
            {
                return commandType;
            }
            set
            {
                commandType = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public Point Pivot
        {
            get
            {
                return pivot;
            }
            set
            {
                pivot = value;
            }
        }

        public Point UpperLeft
        {
            get
            {
                return upperLeft;
            }
            set
            {
                upperLeft = value;
            }
        }

        public object Clone()
        {
            Dictionary<char, string> colorReferences = new Dictionary<char, string>();
            foreach (KeyValuePair<char, string> item in this.colorReferences)
            {
                colorReferences.Add(item.Key, item.Value);
            }

            Collection<string> commands = new Collection<string>();
            foreach (string item in this.commands)
            {
                commands.Add(item);
            }
            SymbolItem symbolItem = new SymbolItem(rcid, boundingBox, colorReferences, commands, commandType, name, pivot, upperLeft);
            return symbolItem;
        }
    }
}
