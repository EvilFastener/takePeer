using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace takePeer
{
    class Program
    {
        static void Main(string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "peers.json";
            string fullPath = Path.Combine(desktopPath, fileName);
            try
            {
                var peer = new PeerOperatrion();
                peer.Addtolist();
                peer.Save2json(fullPath);
            }
            catch (Exception ex)
            {
                File.WriteAllText("failed to take peer because: " + ex.Message, desktopPath);
            }
            

        }
    }
}