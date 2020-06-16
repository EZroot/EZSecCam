using Microsoft.Extensions.Configuration;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(Settings.LOG_CONFIG)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Started debugger");

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

            }));

            //Start webcam thread
            ThreadHandler.Instance.ProcessWithThreadPoolMethod(new WaitCallback(delegate (object state)
            {
                //Show image
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    //Update webcam image
                    DispatcherTimer Timer = new DispatcherTimer();
                    Timer.Tick += (sender, e) =>
                    {
                        BitmapSource frame = Camera.Instance.GetNextFrame();
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
                Settings.detectorType = Settings.DetectorType.Haarcascade;
                Log.Debug("Haarcascade Face Detection {0} {1}", "Status: On", "Detecting faces");
            }
            else
            {
                EnableDetectorButtons();
                Settings.detectorType = Settings.DetectorType.None;
                Log.Debug("Haarcascade Face Detection {0}", "Status: Off");
            }
        }

        private void DNNFaceDetectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DisableDetectorButtons();
            DNNFaceDetectionMenuItem.IsEnabled = true;
            ConfidenceSlider.IsEnabled = true;

            if (DNNFaceDetectionMenuItem.IsChecked)
            {
                Settings.detectorType = Settings.DetectorType.DNN;
                Log.Debug("DNN Face Detection {0}", "Status: On");
            }
            else
            {
                EnableDetectorButtons();
                Settings.detectorType = Settings.DetectorType.None;
                Log.Debug("DNN Face Detection {0}", "Status: Off");
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
            ProgressLabel.Content = "Confidence Detection = " + g.ToString("F2")+"%";
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
            ConnectionSettings.StartServer();
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
            DNNFaceDetectionMenuItem.IsEnabled = false;
            ConfidenceSlider.IsEnabled = false;
        }

        private void EnableDetectorButtons()
        {
            HaarcascadeFaceDetectionMenuItem.IsEnabled = true;
            DNNFaceDetectionMenuItem.IsEnabled = true;
            ConfidenceSlider.IsEnabled = true;
        }
    }
}
