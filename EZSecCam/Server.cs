using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EZSecCam
{
    public class Server
    {
        private Server()
        {
        }

        public static string ServerPort = "2222";
        public static bool listen = true;

        public static async void Start()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

                Log.Debug("Starting TCP listener... {0}:{1} listen={2}", ipAddress, ServerPort, listen);

                TcpListener listener = new TcpListener(ipAddress, int.Parse(ServerPort));

                listener.Start();

                while (listen)
                {
                    if (listener.Pending())
                        await HandleClient(await listener.AcceptTcpClientAsync());
                    else
                        await Task.Delay(100); //<--- timeout
                }

                listener.Stop();
            }
            catch (Exception e)
            {
                Log.Debug("Error: {0}", e.StackTrace);
            }
        }

        private static async Task HandleClient(TcpClient clt)
        {
            using NetworkStream ns = clt.GetStream();
            using StreamReader sr = new StreamReader(ns);
            string msg = await sr.ReadToEndAsync();

            Log.Debug("Received new message ({0} bytes):{1}", msg.Length,msg);
        }

        private static void SendData()
        {
        }
    }
}
