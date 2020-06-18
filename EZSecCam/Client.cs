using Serilog;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace EZSecCam
{
    public class Client
    {
        public static TcpClient socket;
        public static NetworkStream nwStream;
        public static StreamWriter writer;
        public static StreamReader reader;

        public static void Connect()
        {
            //---data to send to the server---
            string textToSend = DateTime.Now.ToString();

            //---create a TCPClient object at the IP and port no.---
            Log.Debug("Connecting.. {0} {1}", ConnectionSettings.ServerIp, ConnectionSettings.ServerPort);
            try
            {
                socket = new TcpClient(ConnectionSettings.ServerIp, int.Parse(ConnectionSettings.ServerPort));
                nwStream = socket.GetStream();
            }
            catch (Exception e)
            {
                Log.Warning("{0}", e.Message);
            }
        }

        public static void SendData(string data, bool endAfterSend = false)
        {
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(data);

            try
            {
                //---send the text---
                Log.Debug("Sending to server : " + data);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                //---read back the text---
                byte[] bytesToRead = new byte[socket.ReceiveBufferSize];
                int bytesRead = nwStream.Read(bytesToRead, 0, socket.ReceiveBufferSize);
                Log.Debug("Received from server: " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }
            //if (endAfterSend)
            //EndConnection();
        }

        public static void EndConnection()
        {
            socket.Close();
        }
    }
}
