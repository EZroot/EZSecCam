using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EZSecCam
{
    public class Camera
    {
        Camera()
        {
        }
        private static Camera instance = null;
        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                        if (instance == null)
                        {
                            instance = new Camera();
                        }
                }
                return instance;
            }
        }

        VideoCapture capture;
        Mat frame;

        public void StartWebcam()
        {
            Console.WriteLine("Starting camera..");

            frame = new Mat();
            capture = new VideoCapture();

            try
            {
                capture.Open(0);

                Console.WriteLine("GetBackendName " + capture.GetBackendName());
                Console.WriteLine("Channel " + capture.Get(VideoCaptureProperties.Channel));
                Console.WriteLine("IsOpened " + capture.IsOpened());
            }
            catch (Exception e)
            {
                Console.Write("Failed to find cam: " + e.Message);
                return;
            }
        }

        public BitmapSource GetNextFrame()
        {
            if (capture.Read(frame))
            {
                //Filter Camera Image
                switch (Settings.filterType)
                {
                    case Settings.FilterType.None:
                        break;
                    case Settings.FilterType.FilterBrightness:
                        Settings.UpdateBrightnessContrast(frame, frame, 50, 20);
                        break;
                }
                
                //Detect people
                switch (Settings.detectorType)
                {
                    case Settings.DetectorType.None:
                        break;
                    case Settings.DetectorType.Haarcascade:
                        var haarCascade = new CascadeClassifier(Settings.HAARCASCADE_FACES);
                        frame = Settings.DetectFace(haarCascade, frame);
                        break;
                }
                return BitmapSourceConverter.ToBitmapSource(frame);
            }
            return null;
        }
    }
}
