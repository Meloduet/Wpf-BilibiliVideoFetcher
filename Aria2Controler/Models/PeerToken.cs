using Newtonsoft.Json;

namespace Aria2Controler.Models
{

    public class PeerToken
    {
        [JsonProperty("peerId")]
        public string PeerId;

        [JsonProperty("ip")]
        public string IP;
    }
}
