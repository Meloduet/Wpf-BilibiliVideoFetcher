using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aria2Controller.JsonRpc
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="http://www.jsonrpc.org/specification"/>
    public static class JsonRpcHelper
    {

        public static readonly string Version = "2.0";

        static Regex s_charsetRegex = new Regex("charset=([^;]*);?");

        /// <summary>
        /// 由已知的网络请求，获取一个用于读取的TextReader实例
        /// </summary>
        /// <param name="req"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static TextReader GetResponseReader(WebRequest req, Encoding e = null)
        {
            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
            }
            catch (WebException ex)
            {
                resp = ex.Response;
                if (resp != null)
                {
                    resp.Close();
                }
                throw;
            }
            var ns = resp?.GetResponseStream();

            Stream stream = ns;
            if (e == null)
            {
                if (resp.SupportsHeaders)
                {
                    var m = s_charsetRegex.Match(resp.ContentType);
                    if (m.Success)
                    {
                        e = Encoding.GetEncoding(m.Groups[1].Value);
                    }
                    var contentEncoding = resp.Headers.Get("Content-Encoding");
                    if (contentEncoding == "gzip")
                    {
                        stream = new GZipStream(ns, CompressionMode.Decompress);
                    }
                    else if (contentEncoding == "deflate")
                    {
                        stream = new DeflateStream(ns, CompressionMode.Decompress);
                    }
                }
                if (e == null)
                {
                    e = Encoding.UTF8;
                }
            }
            return new StreamReader(stream, e);
        }

        /// <summary>
        /// 远程JSON-RPC调用
        /// </summary>
        /// <param name="url">调用URL</param>
        /// <param name="method">方法名</param>
        /// <param name="id">id, 可为null</param>
        /// <param name="parameters">调用参数，非必须</param>
        /// <returns></returns>
        public static object RemoteCall(string url, string method, object id = null, params object[] parameters)
        {
            HttpWebRequest req = CreateRpcRequest(url, method, id, parameters);

            return GetRemoteResult(req).Result;
        }

        /// <summary>
        /// 远程JSON-RPC调用
        /// </summary>
        /// <param name="url">调用URL</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static object RemoteCall(string url, JsonRpcToken token)
        {
            HttpWebRequest req = CreateRpcRequest(url, token);

            return GetRemoteResult(req).Result;
        }

        /// <summary>
        /// 用异步的方式，远程JSON-RPC调用
        /// </summary>
        /// <param name="url">调用URL</param>
        /// <param name="method">方法名</param>
        /// <param name="id">id, 可为null</param>
        /// <param name="parameters">调用参数，非必须</param>
        /// <returns></returns>
        public static async Task<object> RemoteCallAsync(string url, string method, object id = null, params object[] parameters)
        {
            HttpWebRequest req = CreateRpcRequest(url, method, id, parameters);

            return (await GetRemoteResultAsync(req)).Result;
        }

        /// <summary>
        /// 用异步的方式，远程JSON-RPC调用
        /// </summary>
        public static async Task<object> RemoteCallAsync(string url, JsonRpcToken token)
        {
            HttpWebRequest req = CreateRpcRequest(url, token);

            return (await GetRemoteResultAsync(req)).Result;
        }

        private static HttpWebRequest CreateRpcRequest(string url, string data)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(data);
#endif
            HttpWebRequest req = PostFormRequest(url, data);
            return req;
        }

        private static HttpWebRequest CreateRpcRequest(string url, JsonRpcToken token)
        {
            return CreateRpcRequest(url, DumpRequestData(token));
        }

        private static HttpWebRequest CreateRpcRequest(string url, string method, object id, object[] parameters)
        {
            return CreateRpcRequest(url, DumpRequestData(method, id, parameters));
        }

        private static string DumpRequestData(JsonRpcToken token)
        {
            return JsonConvert.SerializeObject(token);
        }

        private static string DumpRequestData(string method, object id, object[] parameters)
        {
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
            }
            var token = new JsonRpcToken(method, id, parameters);

            return DumpRequestData(token);
        }

        private static JsonRpcResult GetRemoteResult(WebRequest req)
        {
            var reader = GetResponseReader(req, Encoding.UTF8);
            var jsonString = reader.ReadToEnd();
            reader.Close();
            var reval = JsonConvert.DeserializeObject<JsonRpcResult>(jsonString);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(jsonString);
            System.Diagnostics.Debug.WriteLine($"{reval.Result.GetType()}");
#endif
            return reval;
        }

        private static async Task<JsonRpcResult> GetRemoteResultAsync(WebRequest req)
        {
            var reader = GetResponseReader(req, Encoding.UTF8);
            var jsonString = reader.ReadToEndAsync();
            var reval = JsonConvert.DeserializeObject<JsonRpcResult>(await jsonString);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(jsonString);
            System.Diagnostics.Debug.WriteLine($"{reval.Result.GetType()}");
#endif
            reader.Close();
            return reval;
        }

        /// <summary>
        /// 返回使用POST请求提交表单
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="formData">表单内容</param>
        /// <returns></returns>
        private static HttpWebRequest PostFormRequest(string url, string formData)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            var buffer = Encoding.UTF8.GetBytes(formData);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.Accept = "gzip, deflate"; // 接受gzip、deflate压缩流
            req.ContentLength = buffer.Length;
            using (var fs = req.GetRequestStream())
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
            return req;
        }

        //return string.Empty;

    }
}
