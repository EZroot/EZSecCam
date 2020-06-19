using Serilog;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace EZSecCam
{
    public class Client
    {
        public static TcpClient socket;
        public static NetworkStream nwStream;
        public static StreamWriter writer;
        public static StreamReader reader;

        public static async void Connect()
        {
            //---data to send to the server---
            string textToSend = DateTime.Now.ToString();
            byte[] derp = Encoding.ASCII.GetBytes(textToSend);
            //---create a TCPClient object at the IP and port no.---
            Log.Debug("Connecting.. {0} {1}", ConnectionSettings.ServerIp, ConnectionSettings.ServerPort);
            try
            {
                socket = new TcpClient(ConnectionSettings.ServerIp, int.Parse(ConnectionSettings.ServerPort));
                nwStream = socket.GetStream();
                for(int i = 0; i < 5; i++)
                    await nwStream.WriteAsync(derp, 0, derp.Length);
                Log.Debug("Client sending {0}",textToSend);
                socket.Close();
            }
            catch (Exception e)
            {
                Log.Warning("{0}", e.Message);
            }
        }

        public static void RecieveData()
        {

        }
    }
}
