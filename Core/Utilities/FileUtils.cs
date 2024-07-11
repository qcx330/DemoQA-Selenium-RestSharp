using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utilities
{
    public class FileUtils
    {
        public static string SanitizeFileName(string fileName)
        {
            fileName = Regex.Replace(fileName, @"[^a-zA-Z0-9_\-]", "_");

            const int maxLength = 80;
            if (fileName.Length > maxLength)
            {
                fileName = fileName.Substring(0, maxLength);
            }

            return fileName;
        }
    }
}