using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Helper
{
    public class FileHelper
    {
        public static string GetFileSizeString(int fileSize)
        {
            var sizeString = "" + fileSize;
            if (fileSize > 1024 * 1024 * 1024)
            {
                sizeString = Math.Round((double)fileSize / (1024 * 1024 * 1024), 2) + " GB";
            }
            else if (fileSize > (1024 * 1024))
            {
                sizeString = Math.Round((double)fileSize / (1024 * 1024), 2) + " MB";
            }
            else if (fileSize > 1024)
            {
                sizeString = Math.Round((double)fileSize / 1024, 2) + " kB";
            }
            else
            {
                sizeString = sizeString + " B";
            }
            return sizeString;
        }
    }
}
