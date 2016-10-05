using Newtonsoft.Json;

namespace Aria2Controller.Models
{

    public class PeerToken
    {
        [JsonProperty("peerId")]
        public string PeerId;

        [JsonProperty("ip")]
        public string IP;
    }
}
