using OpenCvSharp;
using OpenCvSharp.Extensions;
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
            InitializeComponent();

            HaarcascadeFaceDetectionMenuItem.IsEnabled = false;
            FilterMenuItem.IsEnabled = false;

            Log("App Started", "Statis: Idle", "");
        }

        private void StartWebcamMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Start webcam thread
            ThreadHandler.Instance.ProcessWithThreadPoolMethod(new WaitCallback(delegate (object state) 
            { 
                Camera.Instance.StartWebcam();
            }));

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
            Log("Monitoring Camera", "Statis: Success", "Camera Loaded");


            HaarcascadeFaceDetectionMenuItem.IsEnabled = true;
            FilterMenuItem.IsEnabled = true;
            StartWebcamMenuItem.IsEnabled = false;
        }

        private void HaarcascadeFaceDetectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (HaarcascadeFaceDetectionMenuItem.IsChecked)
            {
                Settings.detectorType = Settings.DetectorType.Haarcascade;
                Log("Haarcascade Face Detection", "Status: On", "Detecting faces");
            }
            else
            {
                Settings.detectorType = Settings.DetectorType.None;
                Log("Haarcascade Face Detection", "Status: Off", "");
            }
        }

        private void FilterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (FilterMenuItem.IsChecked)
            {
                Settings.filterType = Settings.FilterType.FilterBrightness;
                Log("Filtering frame", "Status: On", "Filtering Frame");
            }
            else
            {
                Settings.filterType = Settings.FilterType.None;
                Log("Filtering frame", "Status: Off", "");
            }
        }

        public void Log(string progress, string status, string info)
        {
            ProgressLabel.Content = progress;
            StatusLabel.Content = status;
            InfoLabel.Content = info;
        }
    }
}
