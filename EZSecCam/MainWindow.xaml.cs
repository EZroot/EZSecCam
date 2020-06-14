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

        private void Button1_Click(object sender, RoutedEventArgs e)
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
                capture.Open(1);

                Console.WriteLine("GetBackendName " + capture.GetBackendName());
                Console.WriteLine("Channel " + capture.Get(VideoCaptureProperties.Channel));
                Console.WriteLine("Guid " + capture.Guid);
                Console.WriteLine("IsOpened " + capture.IsOpened());
                Console.WriteLine("capture " + capture.ToString());

            }
            catch(Exception e)
            {
                Console.Write("Failed to find cam: "+e.Message);
            }

            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromMilliseconds(30);
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (capture.Read(frame))
            {
                Image1.Source = BitmapSourceConverter.ToBitmapSource(frame);
            }
        }
    }
}
