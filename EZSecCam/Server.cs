using EZServerAPI.Net;
using EZServerAPI.Util;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EZSecCam
{
    public class Server : AsyncTcpClient
	{
		public Server()
		{
			Message += (s, a) => Log.Debug("Server client: {0}", a.Message);
		}

		protected override async Task OnConnectedAsync(bool isReconnected)
		{
			await Task.Delay(500);
			byte[] bytes = Encoding.UTF8.GetBytes("Hello, my name is Server. Talk to me.");
			await Send(new ArraySegment<byte>(bytes, 0, bytes.Length));
		}

		protected override async Task OnReceivedAsync(int count)
		{
			byte[] bytes = ByteBuffer.Dequeue(count);
			string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			Log.Debug("Server client: received: {0}", message);

			bytes = Encoding.UTF8.GetBytes("You said: " + message);
			await Send(new ArraySegment<byte>(bytes, 0, bytes.Length));
		}
	}
	
	public class ServerHandler
	{
		private ServerHandler()
		{ 
		}

		public static Server server;
		public static Client client;

	}
}
