using Newtonsoft.Json;

namespace Aria2Controller.JsonRpc
{
    public interface IMethodToken
    {
        /// <summary>
        /// 要调用的方法名
        /// Method names that begin with the word rpc followed by a period character (U+002E or ASCII 46) are 
        /// reserved for rpc-internal methods and extensions and MUST NOT be used for anything else.
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// RPC调用的参数
        /// 可以被省略
        /// </summary>
        object[] Params { get; set; }
    }

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


    public class MethodToken : IMethodToken
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonRpcToken : JsonRpcBase, IMethodToken
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

        public JsonRpcToken(string method = "", object id = null, params object[] parameters)
        {
            this.JsonRpc = JsonRpcHelper.Version;
            this.Method = method;
            this.Id = id;
            if (parameters.Length > 0)
            {
                this.Params = parameters;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
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

        public T ResultAs<T>()
        {
            return (T)this.Result;
        }
    }

}
