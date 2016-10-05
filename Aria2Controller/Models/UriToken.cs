using Newtonsoft.Json;

namespace Aria2Controller.Models
{
    public enum UriTokenStatus
    {
        /// <summary>
        /// the URI is in use
        /// </summary>
        Used,
        /// <summary>
        /// the URI is still waiting in the queue
        /// </summary>
        Waiting
    }

    public class UriToken
    {
        [JsonProperty("uri")]
        public string Uri;

        [JsonProperty("status")]
        public UriTokenStatus Status;
    }
}
