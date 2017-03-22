using System;
using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    public static class S52RegularInterpolator
    {
        public static string GetHeader(string content)
        {
            return content.Substring(0, 4);
        }

        public static string GetBody(string content)
        {
            return content.Substring(9);
        }

        public static Collection<string> GetSubcontentsBySplitChar(string content, char splitChar)
        {
            Collection<string> subcontents = new Collection<string>();
            string[] contentParts = content.Split(new char[] { splitChar} , StringSplitOptions.RemoveEmptyEntries);

            foreach (string subcontent in contentParts)
            {
                subcontents.Add(subcontent);
            }

            return subcontents;
        }

        public static string GetNewContentByRemoveChar(string content, char removeChar)
        {
            string[] contentPart = content.Split(new char[] { removeChar }, StringSplitOptions.RemoveEmptyEntries);
            return string.Concat(contentPart);
        }

        public static Collection<string> GetSubcontentsBySplitFixesLength(string content, int length)
        {
            Collection<string> subcontents = new Collection<string>();

            for (int i = 0; i < content.Length; i += length)
            {
                string subcontent = content.Substring(i, length);
                subcontents.Add(subcontent);
            }

            return subcontents;
        }
    }
}
