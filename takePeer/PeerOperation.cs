using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace takePeer
{
    internal class PeerOperatrion
    {
        public List<Peer>? Listofpeer { get; set; }
        public string GetPeer()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;

            cmd.StartInfo.Verb = "runas";
            cmd.Start();

            using (StreamWriter stdInputWriter = cmd.StandardInput)
            {
                if (stdInputWriter.BaseStream.CanWrite)
                {
                    stdInputWriter.WriteLine("wg.exe show");
                    stdInputWriter.WriteLine("exit");
                }
            }


            cmd.WaitForExit();


            string response = cmd.StandardOutput.ReadToEnd();
            string errorResponse = cmd.StandardError.ReadToEnd();


            Console.WriteLine("Output:\n" + response);

            if (!string.IsNullOrEmpty(errorResponse))
            {
                Console.WriteLine("Errors:\n" + errorResponse);
            }

            return response;
        }

        public void Addtolist()
        {

            Listofpeer = new List<Peer>();
            Peer peer = new Peer();
            string[] lines = GetPeer().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith("peer:"))
                {
                    peer.Id = line.Replace("peer:", "").Trim();
                }
                if (line.StartsWith("  preshared key:"))
                {
                    peer.PresharedKey = line.Replace("preshared key:", "").Trim();
                }
                if (line.StartsWith("  endpoint:"))
                {
                    peer.Endpoint = line.Replace("endpoint:", "").Trim();
                }
                if (line.StartsWith("  allowed ips:"))
                {
                    string allowedIps = line.Replace("allowed ips:", "").Trim();
                    peer.AllowedIps = new List<string>(allowedIps.Split(','));
                    for (int i = 0; i < peer.AllowedIps.Count; i++)
                    {
                        peer.AllowedIps[i] = peer.AllowedIps[i].Trim();
                    }
                }
                if (line.StartsWith("  latest handshake:"))
                {
                    peer.LatestHandshake = line.Replace("latest handshake:", "").Trim();
                }
                if (line.StartsWith("  transfer:"))
                {
                    peer.Transfer = line.Replace("transfer:", "").Trim();
                    var newpeer = new Peer
                    {
                        Id = peer.Id,
                        PresharedKey = peer.PresharedKey,
                        Endpoint = peer.Endpoint,
                        AllowedIps = peer.AllowedIps,
                        LatestHandshake = peer.LatestHandshake,
                        Transfer = peer.Transfer,
                        HandshakeHistory = new List<string>(),
                    };
                    Listofpeer.Add(newpeer);
                }
            }
        }

        public void Save2json(string filepath)
        {

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull // Ignoruj wartości null podczas serializacji
            };

            List<Peer> existingPeers = new List<Peer>();


            if (File.Exists(filepath))
            {
                string existingJsonString = File.ReadAllText(filepath);
                if (!string.IsNullOrWhiteSpace(existingJsonString))
                {
                    existingPeers = JsonSerializer.Deserialize<List<Peer>>(existingJsonString, options) ?? new List<Peer>();
                }
            }

            if (Listofpeer != null)
            {
                existingPeers.AddRange(Listofpeer);
            }




            var allHandshakes = new List<string>();

            for (int i = 0; i < existingPeers.Count; i++)
            {
                var peer = existingPeers[i];
                if (!string.IsNullOrEmpty(peer.LatestHandshake))
                {
                    allHandshakes.Add(peer.LatestHandshake);
                }
            }

            for (int i = 0; i < existingPeers.Count; i++)
            {
                var peer = existingPeers[i];
                peer.HandshakeHistory = new List<string>();

                for (int j = 0; j < i; j++)
                {
                    if (peer.HandshakeHistory != null && !string.IsNullOrEmpty(existingPeers[j].LatestHandshake))
                    {
                        peer.HandshakeHistory.Add(existingPeers[j].LatestHandshake);
                    }
                }
            }
            string jsonString = JsonSerializer.Serialize(existingPeers, options);
            File.WriteAllText(filepath, jsonString);

            Console.WriteLine("Saved!");
        }


    }

}
