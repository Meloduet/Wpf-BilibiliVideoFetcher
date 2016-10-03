using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Aria2Controler.JsonRpc
{
    public class JsonRpcBase
    {
        /// <summary>
        /// A String specifying the version of the JSON-RPC protocol. MUST be exactly "2.0".
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }

        /// <summary>
        /// An identifier established by the Client that MUST contain a String, Number, or NULL value if included.
        /// If it is not included it is assumed to be a notification. 
        /// The value SHOULD normally not be Null [1] and Numbers SHOULD NOT contain fractional parts [2]
        /// </summary>
        /// <seealso cref="http://www.jsonrpc.org/specification#id1"/>
        /// <seealso cref="http://www.jsonrpc.org/specification#id2"/>
        [JsonProperty("id", Required = Required.AllowNull)]
        public object Id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonRpcToken : JsonRpcBase
    {
        /// <summary>
        /// A String containing the name of the method to be invoked.
        /// Method names that begin with the word rpc followed by a period character (U+002E or ASCII 46) are 
        /// reserved for rpc-internal methods and extensions and MUST NOT be used for anything else.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }
        /// <summary>
        /// A Structured value that holds the parameter values to be used during the invocation of the method. This member MAY be omitted.
        /// </summary>
        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Params { get; set; }

        public JsonRpcToken()
        {
            this.JsonRpc = JsonRpcHelper.Version;
        }
    }

    public class JsonRpcErrorObject
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }

    }

    public class JsonRpcResult : JsonRpcBase
    {
        /// <summary>
        /// This member is REQUIRED on success.
        /// This member MUST NOT exist if there was an error invoking the method.
        /// The value of this member is determined by the method invoked on the Server.
        /// </summary>
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public object Result { get; set; }

        /// <summary>
        /// This member is REQUIRED on error.
        /// This member MUST NOT exist if there was no error triggered during invocation.
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public JsonRpcErrorObject Error { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="http://www.jsonrpc.org/specification"/>
    public static class JsonRpcHelper
    {

        public static readonly string Version = "2.0";

        /// <summary>
        /// 远程JSON-RPC调用
        /// </summary>
        /// <param name="url">调用URL</param>
        /// <param name="method">方法名</param>
        /// <param name="id">id, 可为null</param>
        /// <param name="paramers">调用参数，非必须</param>
        /// <returns></returns>
        public static object RemoteCall(string url, string method, object id = null, params object[] paramers)
        {
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
            }
            var token = new JsonRpcToken() { Method = method, Id = id };
            if (paramers.Length > 0)
            {
                token.Params = paramers;
            }

            string param = JsonConvert.SerializeObject(token);

#if DEBUG

            System.Diagnostics.Debug.WriteLine(param);
#endif

            HttpWebRequest req = PostFormRequest(url, param);

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (var fs = resp.GetResponseStream())
                {
                    var reader = new StreamReader(fs, Encoding.UTF8);
                    var jsonString = reader.ReadToEnd();
                    var reval = JsonConvert.DeserializeObject<JsonRpcResult>(jsonString);
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(jsonString);
                    System.Diagnostics.Debug.WriteLine($"{reval.Result.GetType()}");
#endif
                    return reval.Result;
                }
            }
            catch (WebException e)
            {
                // TODO: 网络异常处理
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw;
            }
            catch (Exception e)
            {
                // TODO: 其它异常处理
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw;
            }

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
            req.ContentLength = buffer.Length;
            using (var fs = req.GetRequestStream())
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
            return req;
        }
    }
}
