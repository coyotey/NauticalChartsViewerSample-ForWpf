using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NauticalChartsViewer
{
    public class LookupItem : ObservableObject, ICloneable
    {
        private string objectClass;
        private int displayPriority;
        private char radarPriority;
        private string styleName;
        private Dictionary<string, string> attributes;
        private Collection<EditionKeyValue> editionAttributes;
        private Collection<string> instructions;
        private Collection<EditionStringField> editionInstructions;
        private string displayCategory;
        private string comment;
        private int rcid;

        public LookupItem()
        {
            objectClass = string.Empty;
            displayPriority = 0;
            radarPriority = new char();
            styleName = string.Empty;
            attributes = new Dictionary<string, string>();
            instructions = new Collection<string>();
            displayCategory = string.Empty;
            comment = string.Empty;
        }

        public LookupItem(int rcid, string objectClass, int displayPriority,
            char radarPriority, string styleName, Dictionary<string, string> attributes,
            Collection<string> instructions, string displayCategory, string comment)
        {
            this.rcid = rcid;
            this.objectClass = objectClass;
            this.displayPriority = displayPriority;
            this.radarPriority = radarPriority;
            this.styleName = styleName;
            this.attributes = attributes;
            this.instructions = instructions;
            this.displayCategory = displayCategory;
            this.comment = comment;
        }

        public int Rcid
        {
            get { return rcid; }
            set { rcid = value; }
        }

        public string ObjectClass
        {
            get { return objectClass; }
            set { objectClass = value; }
        }

        public int DisplayPriority
        {
            get { return displayPriority; }
            set
            {
                displayPriority = value;
                RaisePropertyChanged("DisplayPriority");
            }
        }

        public char RadarPriority
        {
            get { return radarPriority; }
            set
            {
                radarPriority = value;
                RaisePropertyChanged("RadarPriority");
            }
        }

        public string StyleName
        {
            get
            {
                return styleName;
            }
            set
            {
                styleName = value;
            }
        }

        public Dictionary<string, string> Attributes
        {
            get
            {
                return attributes;
            }
            set
            {
                attributes = value;
            }
        }

        public Collection<EditionKeyValue> EditionAttributes
        {
            get
            {
                if (editionAttributes == null)
                {
                    editionAttributes = new Collection<EditionKeyValue>();
                    foreach (KeyValuePair<string, string> attribute in attributes)
                    {
                        editionAttributes.Add(new EditionKeyValue(attribute.Key, attribute.Value));
                    }
                }
                return editionAttributes;
            }
        }

        public Collection<string> Instructions
        {
            get
            {
                return instructions;
            }
            set
            {
                instructions = value;
            }
        }

        public Collection<EditionStringField> EditionInstructions
        {
            get
            {
                if (editionInstructions == null)
                {
                    editionInstructions = new Collection<EditionStringField>();
                    foreach (string command in instructions)
                    {
                        editionInstructions.Add(new EditionStringField(command));
                    }
                }
                return editionInstructions;
            }
        }

        public string DisplayCategory
        {
            get { return displayCategory; }
            set
            {
                displayCategory = value;
                RaisePropertyChanged("DisplayCategory");
            }
        }

        public string Comments
        {
            get { return comment; }
            set { comment = value; }
        }

        public object Clone()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in this.attributes)
            {
                attributes.Add(item.Key, item.Value);
            }

            Collection<string> instructions = new Collection<string>();
            foreach (string item in this.instructions)
            {
                instructions.Add(item);
            }
            return new LookupItem(rcid, objectClass, displayPriority, radarPriority, styleName, attributes, instructions, displayCategory, comment);
        }
    }
}
