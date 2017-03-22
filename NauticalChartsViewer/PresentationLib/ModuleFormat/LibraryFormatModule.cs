
namespace ThinkGeo.MapSuite
{
    public abstract class LibraryFormatModule
    {
        // MODN
        private string moduleName;
        // RCID
        private int recordId;
        // STAT
        private string status;

        protected LibraryFormatModule()
        {
            moduleName = string.Empty;
            recordId = 0;
            status = string.Empty;
        }

        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        public int Rcid
        {
            get { return recordId; }
            set { recordId = value; }
        }

        internal string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
