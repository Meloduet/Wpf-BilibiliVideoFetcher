using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            return GetTextFromUri(uri, Encoding.UTF8);
        }

        public static string GetTextFromUri(string uri, Encoding encoding)
        {
            try
            {
                var buffer = GetBytesFromUri(uri);
                return encoding.GetString(buffer);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        /// <exception cref="SocketException"></exception>
        public static byte[] GetBytesFromUri(string uri)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.82 Safari/537.36");

                    client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate"); // 在服务器支持的情况下，接受gzip或deflate压缩流
                    var ns = client.OpenRead(uri);
                    Stream stream = ns;
                    var contentEncoding = client.ResponseHeaders.Get("Content-Encoding");
                    if (contentEncoding == "gzip")
                    {
                        stream = new GZipStream(ns, CompressionMode.Decompress);
                    }
                    else if (contentEncoding == "deflate")
                    {
                        stream = new DeflateStream(ns, CompressionMode.Decompress);
                    }
                    MemoryStream ms = new MemoryStream();
                    stream.CopyTo(ms);
                    return ms.ToArray();
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
