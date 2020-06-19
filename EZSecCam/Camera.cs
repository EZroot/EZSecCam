using OpenCvSharp;
using Serilog;
using System;
using System.Threading;

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
            Log.Debug("Starting Camera... {0}", "Channel 0");

            frame = new Mat();
            capture = new VideoCapture();

            try
            {
                capture.Open(0);

                Log.Debug("GetBackendName {0}", capture.GetBackendName());
                Log.Debug("Channel  {0}", capture.Get(VideoCaptureProperties.Channel));
                Log.Debug("IsOpened  {0}", capture.IsOpened());
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to find cam {0} ", e.Message);
                return;
            }
        }

        public Mat GetNextFrame()
        {
            try
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
                    switch (Settings.DetectorType1)
                    {
                        case Settings.DetectorType.None:
                            break;
                        case Settings.DetectorType.Haarcascade:
                            var haarCascade = new CascadeClassifier(Settings.HAARCASCADE_FACES);
                            frame = Settings.DetectFace(haarCascade, frame);
                            break;
                        case Settings.DetectorType.Caffe:
                            frame = Settings.DetectFaceCaffe(frame);
                            break;
                        case Settings.DetectorType.YoloV3:
                            frame = Settings.DetectFaceYoloV3(frame);
                            break;
                    }
                    return frame;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }
            Log.Warning("Failed to grab frame {0}", "returning new Mat()");
            return new Mat();
        }
    }
}
