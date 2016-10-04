using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Helper
{
    public class UrlHelper
    {
        public static string FixUrl(string url)
        {
            if(!(url.StartsWith("http://")|| url.StartsWith("https://")))
            {
                return "http://" + url;
            }else
            {
                return url;
            }
        }
    }
}
