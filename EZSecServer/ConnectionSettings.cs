using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;

namespace EZSecServer
{
    public class ConnectionSettings
    {
        public const string CONNECTION_CONFIG_PATH = "Data/connection_config.json";
        public const string LOG_CONFIG = "Data/appsettings.json";

        public static string ServerIp = "127.0.0.1";
        public static string PublicKey = "";
        public static string PublicKeyExp = "";
        public static string ServerPort = "2222";

        public static void WriteConfig()
        {
            ServerJsonConfig config = new ServerJsonConfig(ServerIp, PublicKey, PublicKeyExp);

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(CONNECTION_CONFIG_PATH))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, config);
            }
            Log.Debug("Config Saved! {0} {1} {2}", ServerIp, PublicKey, PublicKeyExp);
        }

        public static void ReadConfig()
        {
            ServerJsonConfig config;

            if (!File.Exists(CONNECTION_CONFIG_PATH))
            {
                Log.Warning("Failed to find file. Does it Exist? {0}", CONNECTION_CONFIG_PATH);
                return;
            }

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(CONNECTION_CONFIG_PATH))
            {
                JsonSerializer serializer = new JsonSerializer();
                config = (ServerJsonConfig)serializer.Deserialize(file, typeof(ServerJsonConfig));
            }

            ServerIp = config.ServerIp;
            PublicKey = config.PublicKey;
            PublicKeyExp = config.PublicKeyExp;

            Log.Debug("Config file read {0} {1} {2}", ServerIp, PublicKey, PublicKeyExp);
        }

        [Serializable]
        class ServerJsonConfig
        {
            public string ServerIp = "127.0.0.1";
            public string PublicKey = "";
            public string PublicKeyExp = "";

            public ServerJsonConfig(string ip, string key, string exp)
            {
                ServerIp = ip;
                PublicKey = key;
                PublicKeyExp = exp;
            }
        }
    }
}
