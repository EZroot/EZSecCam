using Microsoft.Extensions.Configuration;
using OpenCvSharp.Extensions;
using Serilog;
using System;
using System.Threading;
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
                        BitmapSource frame = BitmapSourceConverter.ToBitmapSource(Camera.Instance.GetNextFrame());
                        WebcamImage.Source = frame;
                    };
                    Timer.Interval = TimeSpan.FromMilliseconds(30);
                    Timer.Start();
                }));
            }));

            EnableDetectorButtons();
            EnableFilterButtons();
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
            ConnectionWindow connectWindow = new ConnectionWindow();
            connectWindow.Show();
        }

        public void StartServerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Launch EZServer.exe binary
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
