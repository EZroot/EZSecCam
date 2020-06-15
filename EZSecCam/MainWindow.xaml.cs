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
        VideoCapture capture;
        Mat frame;
        private Thread t;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void StartCameraButton_Click(object sender, RoutedEventArgs e)
        {
            StartWebcam();
        }

        private void StartWebcam()
        {
            t = new Thread(delegate ()
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(RunWebcam));
            });
            t.Start();
        }

        private void RunWebcam()
        {
            frame = new Mat();
            capture = new VideoCapture();

            try
            {
                capture.Open(0);

                Console.WriteLine("GetBackendName " + capture.GetBackendName());
                Console.WriteLine("Channel " + capture.Get(VideoCaptureProperties.Channel));
                Console.WriteLine("Guid " + capture.Guid);
                Console.WriteLine("IsOpened " + capture.IsOpened());
            }
            catch (Exception e)
            {
                Console.Write("Failed to find cam: "+e.Message);
                return;
            }

            //TODO: Create thread to make webcam faster
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += CameraFrameUpdate;
            Timer.Interval = TimeSpan.FromMilliseconds(230);
            Timer.Start();

            //TODO: Create thread to make face detection faster
            DispatcherTimer Timer2 = new DispatcherTimer();
            Timer2.Tick += CascadeFaceDetectorFrameUpdate;
            Timer2.Interval = TimeSpan.FromMilliseconds(30);
            Timer2.Start();
        }

        private void CameraFrameUpdate(object sender, EventArgs e)
        {
            if (capture.Read(frame))
            {
                switch(Settings.filterType)
                {
                    case Settings.FilterType.None:
                        break;
                    case Settings.FilterType.FilterBrightness:
                        Settings.UpdateBrightnessContrast(frame, frame, 50, 20);
                        break;
                }

                WebcamImage.Source = BitmapSourceConverter.ToBitmapSource(frame);
            }
        }

        private void CascadeFaceDetectorFrameUpdate(object sender, EventArgs e)
        {
            if (capture.Read(frame))
            {
                var haarCascade = new CascadeClassifier(Settings.HarrcascadePath);
                Mat haarResult = Settings.DetectFace(haarCascade, frame);

                HistogramImage.Source = BitmapSourceConverter.ToBitmapSource(haarResult);
            }
        }

        private void FilterCamButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.filterType = Settings.FilterType.FilterBrightness;
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.filterType = Settings.FilterType.None;
        }

        private void HistogramButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
