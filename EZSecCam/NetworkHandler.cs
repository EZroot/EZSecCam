using EZServerAPI.Net;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EZSecCam
{
    public class NetworkHandler
    {
        public static Client client;
        public static AsyncTcpListener server;

        public static BitmapSource frameSource;

        public static void StartServer()
        {
            server = new AsyncTcpListener
            {
                IPAddress = IPAddress.IPv6Any,
                Port = 2222,
                ClientConnectedCallback = tcpClient =>
                    new AsyncTcpClient
                    {
                        ServerTcpClient = tcpClient,
                        ConnectedCallback = async (serverClient, isReconnected) =>
                        {
                            await Task.Delay(500);
                            byte[] bytes = Encoding.UTF8.GetBytes($"Hello, {tcpClient.Client.RemoteEndPoint}, my name is Server. Talk to me.");
                            await serverClient.Send(new ArraySegment<byte>(bytes, 0, bytes.Length));
                        },
                        ReceivedCallback = async (serverClient, count) =>
                        {
                            byte[] bytes = serverClient.ByteBuffer.Dequeue(count);
                            string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                            Log.Debug("Server client: received: {0}", message);

                            if (message.Contains("REQ"))
                                bytes = Encoding.UTF8.GetBytes("Server=>Webcam.GetFrame()");
                            else
                                bytes = Encoding.UTF8.GetBytes("No frame requested=>" + message);

                            await serverClient.Send(new ArraySegment<byte>(bytes, 0, bytes.Length));

                            if (message == "bye")
                            {
                                    // Let the server close the connection
                                    serverClient.Disconnect();
                            }
                        }
                    }.RunAsync()
            };
            server.Message += (s, a) => Log.Debug("Server: {0}", a.Message);
            server.RunAsync().GetAwaiter().GetResult();
        }

        public static void StartClient()
        {
            client = new Client
            {
                IPAddress = IPAddress.IPv6Loopback,
                Port = 2222
                //AutoReconnect = true
            };
            client.RunAsync().GetAwaiter().GetResult();
        }
    }
}
