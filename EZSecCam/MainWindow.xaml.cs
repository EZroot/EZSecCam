using EZServerAPI.Net;
using Microsoft.Extensions.Configuration;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Serilog;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Window = System.Windows.Window;

namespace EZSecCam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            #region SeriLogger
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(Settings.LOG_CONFIG)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Started debugger");
            #endregion
            //Settings.ReadConfig();
            InitializeComponent();

            DisableDetectorButtons();
            DisableFilterButtons();

            Log.Debug("App Started {0}", "Statis: Idle");
        }

        private void StartWebcamMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Start webcam thread
            ThreadHandler.Instance.ProcessWithThreadPoolMethod(new WaitCallback(delegate (object state)
            {
                Camera.Instance.StartWebcam();

                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    //Update webcam image
                    DispatcherTimer Timer = new DispatcherTimer();
                    Timer.Tick += (sender, e) =>
                    {
                        Mat nextFrame = Camera.Instance.GetNextFrame();
                        BitmapSource frame = BitmapSourceConverter.ToBitmapSource(nextFrame);
                        /*if (Client.isConnected) 
                        {
                            Stopwatch s = new Stopwatch();
                            s.Start();
                            Cv2.Resize(nextFrame, nextFrame, new OpenCvSharp.Size(640, 480));
                            Client.SendData(Client.BitmapSourceToArray(BitmapSourceConverter.ToBitmapSource(nextFrame)));
                            s.Stop();
                            Log.Debug("Resize Client data runtime: {0}ms", s.ElapsedMilliseconds);
                            Client.Disconnect();
                        }*/
                        WebcamImage.Source = frame;
                    };
                    Timer.Interval = TimeSpan.FromMilliseconds(1000);
                    Timer.Start();
                }));
            }));

            EnableDetectorButtons();
            EnableFilterButtons();
            StopServerMenuItem.IsEnabled = false;
            StartWebcamMenuItem.IsEnabled = false;
        }


        private void HaarcascadeFaceDetectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisableDetectorButtons();
            HaarcascadeFaceDetectionMenuItem.IsEnabled = true;

            if (HaarcascadeFaceDetectionMenuItem.IsChecked)
            {
                Settings.DetectorType1 = Settings.DetectorType.Haarcascade;
                Log.Debug("Haarcascade Face Detection {0} {1}", "Status: On", "Detecting faces");
            }
            else
            {
                EnableDetectorButtons();
                Settings.DetectorType1 = Settings.DetectorType.None;
                Log.Debug("Haarcascade Face Detection {0}", "Status: Off");
            }
        }

        private void CaffeDNNFaceDetectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisableDetectorButtons();
            CaffeDNNFaceDetectionMenuItem.IsEnabled = true;
            ConfidenceSlider.IsEnabled = true;

            if (CaffeDNNFaceDetectionMenuItem.IsChecked)
            {
                Settings.DetectorType1 = Settings.DetectorType.Caffe;
                Log.Debug("Caffe DNN Face Detection {0}", "Status: On");
            }
            else
            {
                EnableDetectorButtons();
                Settings.DetectorType1 = Settings.DetectorType.None;
                Log.Debug("Caffe DNN Face Detection {0}", "Status: Off");
            }
        }

        private void YoloV3DNNFaceDetectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisableDetectorButtons();
            YoloV3DNNFaceDetectionMenuItem.IsEnabled = true;
            ConfidenceSlider.IsEnabled = true;

            if (YoloV3DNNFaceDetectionMenuItem.IsChecked)
            {
                Settings.DetectorType1 = Settings.DetectorType.YoloV3;
                Log.Debug("YoloV3 (You only look once) DNN Face Detection {0}", "Status: On");
            }
            else
            {
                EnableDetectorButtons();
                Settings.DetectorType1 = Settings.DetectorType.None;
                Log.Debug("YoloV3 (You only look once) DNN Face Detection {0}", "Status: Off");
            }
        }

        private void FilterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisableFilterButtons();
            FilterMenuItem.IsEnabled = true;

            if (FilterMenuItem.IsChecked)
            {
                Settings.filterType = Settings.FilterType.FilterBrightness;
                Log.Debug("Filtering frame {0} {1}", "Status: On", "Filtering Frame");
            }
            else
            {
                EnableFilterButtons();
                Settings.filterType = Settings.FilterType.None;
                Log.Debug("Filtering frame {0}", "Status: Off");
            }
        }

        private void ConfidenceSlider_OnChange(object sender, RoutedEventArgs e)
        {
            float g = (float)ConfidenceSlider.Value;
            Settings.Confidence = g * 0.01f;
            ProgressLabel.Content = "Confidence Detection = " + g.ToString("F2") + "%";
        }

        private void ConnectSettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConnectionWindow connectWindow = new ConnectionWindow();
            connectWindow.Show();
        }

        private void ConnectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ThreadHandler.Instance.ProcessWithThreadPoolMethod(new WaitCallback(delegate (object sender)
            {
                Client client = new Client
                {
                    IPAddress = IPAddress.IPv6Loopback,
                    Port = 2222,
                    ConnectedCallback = async (c, isReconnected) =>
                    {
                        await c.WaitAsync();   // Wait for server banner
                        await Task.Delay(50);   // Let the banner land in the console window
                        Log.Debug("Client: type a message at the prompt, or empty to quit (server shutdown in 10s)");
                        while (true)
                        {
                            // User input
                            string enteredMessage = "This is a test";
                            byte[] bytes = Encoding.UTF8.GetBytes(enteredMessage);
                            await c.Send(new ArraySegment<byte>(bytes, 0, bytes.Length));

                            // Wait for server response or closed connection
                            await c.ByteBuffer.WaitAsync();
                            if (c.IsClosing)
                            {
                                break;
                            }
                        }
                    },
                    ReceivedCallback = (c, count) =>
                    {
                        byte[] bytes = c.ByteBuffer.Dequeue(count);
                        string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        Log.Debug("Client: received: " + message);
                        return Task.CompletedTask;
                    }
                    //AutoReconnect = true
                };
                client.RunAsync().GetAwaiter().GetResult();
            }));
            //Client.Disconnect();
        }

        public void StartServerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StopServerMenuItem.IsEnabled = true;
            StartServerMenuItem.IsEnabled = false;

            ThreadHandler.Instance.ProcessWithThreadPoolMethod(new WaitCallback(delegate (object sender)
            {
                var server = new AsyncTcpListener
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

                                bytes = Encoding.UTF8.GetBytes("You said: " + message);
                                await serverClient.Send(new ArraySegment<byte>(bytes, 0, bytes.Length));

                                if (message == "bye")
                                {
                                // Let the server close the connection
                                serverClient.Disconnect();
                                }
                            }
                        }.RunAsync()
                };
                server.Message += (s, a) => Log.Debug("Server: {0}",a.Message);
                server.RunAsync().GetAwaiter().GetResult();
            }));
            //Server.Start();
        }

        public void StopServerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StopServerMenuItem.IsEnabled = false ;
            StartServerMenuItem.IsEnabled = true;
        }

        private void DisableFilterButtons()
        {
            FilterMenuItem.IsEnabled = false;
        }

        private void EnableFilterButtons()
        {
            FilterMenuItem.IsEnabled = true;
        }

        private void DisableDetectorButtons()
        {
            HaarcascadeFaceDetectionMenuItem.IsEnabled = false;
            CaffeDNNFaceDetectionMenuItem.IsEnabled = false;
            YoloV3DNNFaceDetectionMenuItem.IsEnabled = false;
            ConfidenceSlider.IsEnabled = false;
        }

        private void EnableDetectorButtons()
        {
            HaarcascadeFaceDetectionMenuItem.IsEnabled = true;
            CaffeDNNFaceDetectionMenuItem.IsEnabled = true;
            YoloV3DNNFaceDetectionMenuItem.IsEnabled = true;
            ConfidenceSlider.IsEnabled = true;
        }
    }
}
