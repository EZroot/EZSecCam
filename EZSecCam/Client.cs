using EZServerAPI.Net;
using Serilog;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EZSecCam
{
    public class Client : AsyncTcpClient
	{
		protected override async Task OnConnectedAsync(bool isReconnected)
		{
			await WaitAsync();   // Wait for server banner
			await Task.Delay(50);   // Let the banner land in the console window
			Console.WriteLine("Client: type a message at the prompt, or empty to quit (server shutdown in 10s)");
			while (true)
			{
				bool isClosing = false;

				if (isClosing)
				{
					// Closed connection
					break;
				}

				// User input
				string enteredMessage = "This is a sent string";
				byte[] bytes = Encoding.UTF8.GetBytes(enteredMessage);
				await Send(new ArraySegment<byte>(bytes, 0, bytes.Length));

				// Wait for server response or closed connection
				await ByteBuffer.WaitAsync();
				if (IsClosing)
				{
					break;
				}
			}
		}

		protected override Task OnReceivedAsync(int count)
		{
			byte[] bytes = ByteBuffer.Dequeue(count);
			string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			Log.Debug("Client: received: {0}", message);
			return Task.CompletedTask;
		}
		public static byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            // Stride = (width) x (bytes per pixel)
            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(pixels, stride, 0);

            return pixels;
        }
    }
}
