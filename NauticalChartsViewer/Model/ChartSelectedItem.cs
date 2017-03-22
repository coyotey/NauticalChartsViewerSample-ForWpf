using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NauticalChartsViewer
{
    public class ChartSelectedItem : ObservableObject
    {
        private string name;
        private int updatedFilesCount;
        private string fullName;

        private static Regex regex = new Regex(@"\d{3}$", RegexOptions.Compiled);

        private IEnumerable<FeatureInfo> selectedFeatures;

        public ChartSelectedItem(string fullName, IEnumerable<FeatureInfo> selectedFeatures)
        {
            this.fullName = fullName;
            this.name = Path.GetFileName(fullName);
            this.selectedFeatures = selectedFeatures;
        }

        public string FullName
        {
            get { return fullName; }
        }

        public string Name
        {
            get { return name; }
        }

        public IEnumerable<FeatureInfo> FeatureInfoItems
        {
            get { return selectedFeatures; }
        }

        public int UpdatedFilesCount
        {
            get
            {
                if (updatedFilesCount == 0 && !string.IsNullOrEmpty(fullName))
                {
                    string dir = Path.GetDirectoryName(fullName);
                    string fileNamewithoutEx = Path.GetFileNameWithoutExtension(fullName);
                    string[] files = Directory.GetFiles(dir, string.Format("{0}.*", fileNamewithoutEx));

                    foreach (string file in files)
                    {
                        if (string.Compare(file, fullName, System.StringComparison.InvariantCultureIgnoreCase) != 0)
                        {
                            if (regex.IsMatch(file))
                            {
                                updatedFilesCount++;
                            }
                        }
                    }
                }

                return updatedFilesCount;
            }
        }

    }
}
