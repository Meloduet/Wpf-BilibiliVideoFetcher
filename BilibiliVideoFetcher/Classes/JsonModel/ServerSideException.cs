using BilibiliVideoFetcher.Process;
using Newtonsoft.Json;

namespace BilibiliVideoFetcher.Classes.JsonModel
{
    /// <summary>
    /// 服务器端返回的错误码
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServerSideException : FetchException
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("error")]
        public string ErrorMessage { get; set; }

        public ServerSideException()
        {
        }

        public ServerSideException(int code, string message) : base(message)
        {
            this.Code = code;
        }

        public override string ToString()
        {
            return $"获取视频信息失败, 错误代号: {Code}, error: {ErrorMessage}";
        }
    }
}
