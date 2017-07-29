using BilibiliVideoFetcher.Data;
using System;

namespace BilibiliVideoFetcher.Helper
{
    public static class BiliBiliApiHelper
    {
        /// <summary>
        /// 对queryString 进行签名的 AccessKey
        /// </summary>
        /// <param name="queryString">要签名的对queryString</param>
        /// <returns>签名后的queryString</returns>
        public static string SignQueryParams(string queryString)
        {
            string sign = (queryString + AdvanceSettings.AccessKey).CalculateMD5();
            return $"{queryString}&sign={sign}";
        }

        public static bool SignQueryParams(object p)
        {
            throw new NotImplementedException();
        }
    }

}
