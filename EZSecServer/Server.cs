using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EZSecServer
{
    public class Server
    {
        public static void Run()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(ConnectionSettings.LOG_CONFIG)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Started debugger");

            ConnectionSettings.ReadConfig();

            StartListening();
        }

        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening()
        {
            IPAddress localAdd = IPAddress.Parse(ConnectionSettings.ServerIp);
            TcpListener listener = new TcpListener(localAdd, int.Parse(ConnectionSettings.ServerPort));
            Log.Debug("Listening... {0} {1}", ConnectionSettings.ServerIp, ConnectionSettings.ServerPort);
            listener.Start();

            while (true)
            {
                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Log.Debug("Received from client {0}", dataReceived);

                //---write back the text to the client---
                Log.Debug("Sending to client {0}", dataReceived);
                nwStream.Write(buffer, 0, bytesRead);
            }
            listener.Stop();
            Log.Debug("Server stopped.");
        }
    }
}
