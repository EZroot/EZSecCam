using OpenCvSharp;
using OpenCvSharp.Extensions;
using Serilog;
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
            Log.Debug("Starting Camera... {0}","Channel 0");

            frame = new Mat();
            capture = new VideoCapture();

            try
            {
                capture.Open(0);

                Log.Debug("GetBackendName {0}",  capture.GetBackendName());
                Log.Debug("Channel  {0}", capture.Get(VideoCaptureProperties.Channel));
                Log.Debug("IsOpened  {0}", capture.IsOpened());
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to find cam {0} ", e.Message);
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
                    case Settings.DetectorType.DNN:
                        frame = Settings.DetectFaceDNN(frame);
                        break;
                }
                return BitmapSourceConverter.ToBitmapSource(frame);
            }
            return null;
        }
    }
}
