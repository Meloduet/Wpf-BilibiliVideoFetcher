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
                    client.Headers.Add(HttpRequestHeader.UserAgent,"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.82 Safari/537.36");
                    var bytes = client.DownloadData(uri);
                    var encoding = Encoding.UTF8;
                    return encoding.GetString(bytes);
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
        public static byte[] GetBytesFromUri(string uri)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.82 Safari/537.36");
                    var s = client.DownloadData(uri);
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
