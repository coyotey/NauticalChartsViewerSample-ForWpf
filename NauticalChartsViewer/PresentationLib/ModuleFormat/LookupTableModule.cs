using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    public class LookupTableModule : LibraryFormatModule
    {
        // OBCL
        private string objectClass;

        // FTYP
        private char objectType;

        // DPRI
        private int displayPriority;

        // RPRI
        private char radarPriority;

        // TNAM
        private string showStyleName;

        // all ATTC, key = ATTL, value = ATTV
        private Dictionary<string, string> attributes;

        // all SINS, splits by ';'
        private Collection<string> instructions;

        // DSCN
        private string displayCategory;

        // LUED
        private string tableComment;

        public LookupTableModule()
        {
            objectClass = string.Empty;
            objectType = new char();
            displayPriority = 0;
            radarPriority = new char();
            showStyleName = string.Empty;
            attributes = new Dictionary<string, string>();
            instructions = new Collection<string>();
            displayCategory = string.Empty;
            tableComment = string.Empty;
        }

        public string ObjectClass
        {
            get { return objectClass; }
            set { objectClass = value; }
        }

        public char GeometryType
        {
            get { return objectType; }
            set { objectType = value; }
        }

        public int DisplayPriority
        {
            get { return displayPriority; }
            set { displayPriority = value; }
        }

        public char RadarPriority
        {
            get { return radarPriority; }
            set { radarPriority = value; }
        }

        public string DisplayType
        {
            get { return showStyleName; }
            set { showStyleName = value; }
        }

        public Dictionary<string, string> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public Collection<string> Instructions
        {
            get { return instructions; }
            set { instructions = value; }
        }

        public string DisplayCategory
        {
            get { return displayCategory; }
            set { displayCategory = value; }
        }

        public string Comments
        {
            get { return tableComment; }
            set { tableComment = value; }
        }
    }
}