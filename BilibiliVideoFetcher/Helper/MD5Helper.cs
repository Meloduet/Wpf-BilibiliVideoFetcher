using System.Text;
using System.Security.Cryptography;

namespace BilibiliVideoFetcher.Helper
{
    public static class MD5Helper
    {
        /// <summary>
        /// 获取/设置 对字符串进行解码时使用的 编/解码器，默认为 Encoding.UTF8
        /// </summary>        
        public static Encoding Encoding { get; set; }

        /// <summary>
        /// 获取/设置 用于计算MD5值的MD5对象
        /// </summary>
        public static MD5 MD5Cryptor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CalculateMD5(this byte[] data)
        {
            return MD5Helper.MD5Cryptor.ComputeHash(data);
        }

        /// <summary>
        /// 计算字符串的MD5值，返回 小写的32位长度字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CalculateMD5(this string data)
        {
            byte[] result = MD5Helper.Encoding.GetBytes(data);
            byte[] output = result.CalculateMD5();
            StringBuilder sb = new StringBuilder(32);
            foreach (var byt in output)
            {
                sb.AppendFormat("{0:x2}", byt);
            }
            return sb.ToString();
        }

        static MD5Helper()
        {
            MD5Helper.MD5Cryptor = new MD5CryptoServiceProvider();
            MD5Helper.Encoding = Encoding.UTF8;
        }
    }

}