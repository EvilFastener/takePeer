using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace takePeer
{
    public class Peer
    {
        public string? Id { get; set; }

        [JsonPropertyName("preshared_key")]
        public string? PresharedKey { get; set; }

        public string? Endpoint { get; set; }

        [JsonPropertyName("allowed_ips")]
        public List<string>? AllowedIps { get; set; }

        [JsonPropertyName("latest_handshake")]
        public string? LatestHandshake { get; set; }

        public string? Transfer { get; set; }
        [JsonPropertyName("handshake_history")]
        public List<string>? HandshakeHistory { get; set; }
    }
}
