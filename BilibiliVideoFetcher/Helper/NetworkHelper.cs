using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Helper
{
    class NetworkHelper
    {
        public static string GetTextFromUri(string uri)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string s = client.DownloadString(uri);
                    return s;
                }
                //通常是404
                catch (System.Net.WebException e)
                {                    
                    //Logger.Log("Exception at GetXmlFromUri(" + uri + "), msg: " + e.Message);
                    throw;

                }
                //通常是超时
                catch (System.Net.Sockets.SocketException e)
                {                    
                    //Logger.Log("Exception at GetXmlFromUri(" + uri + "), msg: " + e.Message);
                    throw;
                }

            }
        }
    }
}
