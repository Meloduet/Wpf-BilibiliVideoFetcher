using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Helper
{
    public class FileHelper
    {
        public const int IEC_UNIT_BYTE = 1;
        /// <summary>
        /// IEC标准 1 KiB = 1,024 bytes
        /// </summary>
        public const int IEC_UNIT_KB = IEC_UNIT_BYTE << 10;
        /// <summary>
        /// IEC标准 1 MiB = 1,024 KiB = 1,048,576 bytes
        /// </summary>
        public const int IEC_UNIT_MB = IEC_UNIT_KB << 10;
        /// <summary>
        /// IEC标准 1 GiB = 1,024 MiB = 1,048,576 KiB = 1,073,741,824 bytes
        /// </summary>
        public const int IEC_UNIT_GB = IEC_UNIT_MB << 10;
        /// <summary>
        /// IEC标准 1 TiB = 1,024 GiB = 1,048,576 MiB = 1,073,741,824 KiB = 1,099,511,627,776 bytes
        /// </summary>
        public const long IEC_UNIT_TB = (long)IEC_UNIT_GB << 10;

        public static string GetFileSizeString(int fileSize)
        {
            var sizeString = string.Empty;
            if (fileSize > IEC_UNIT_GB)
            {
                sizeString = Math.Round((double)fileSize / (IEC_UNIT_GB), 2) + " GB";
            }
            else if (fileSize > (IEC_UNIT_MB))
            {
                sizeString = Math.Round((double)fileSize / (IEC_UNIT_MB), 2) + " MB";
            }
            else if (fileSize > 1024)
            {
                sizeString = Math.Round((double)fileSize / IEC_UNIT_KB, 2) + " KB";
            }
            else
            {
                sizeString = $"{sizeString} B";
            }
            return sizeString;
        }

        private static readonly char[] _invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            /* new[]{
               '"','<','>','|','\0','\u0001','\u0002','\u0003','\u0004',
               '\u0005','\u0006','\a','\b','\t','\n','\v','\f','\r',
               '\u000e','\u000f','\u0010','\u0011','\u0012','\u0013',
               '\u0014','\u0015','\u0016','\u0017','\u0018','\u0019',
               '\u001a','\u001b','\u001c','\u001d','\u001e','\u001f',
               ':','*','?','\\','/'};
            */

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
