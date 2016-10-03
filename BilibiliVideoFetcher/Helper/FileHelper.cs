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

        private static readonly char[] _invalidFileNameChars = new[]
        {'"','<','>','|','\0','\u0001','\u0002','\u0003','\u0004',
            '\u0005','\u0006','\a','\b','\t','\n','\v','\f','\r',
            '\u000e','\u000f','\u0010','\u0011','\u0012','\u0013',
            '\u0014','\u0015','\u0016','\u0017','\u0018','\u0019',
            '\u001a','\u001b','\u001c','\u001d','\u001e','\u001f',
            ':','*','?','\\','/'};

        //过滤方法

        public static string CleanInvalidFileName(string fileName)
        {

            fileName = fileName + "";
            fileName = _invalidFileNameChars.Aggregate(fileName, (current, c) => current.Replace(c + "", ""));
            if (fileName.Length > 1)
                if (fileName[0] == '.')
                    fileName = "dot" + fileName.TrimStart('.');
            return fileName;

        }
    }
}
