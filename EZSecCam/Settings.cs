using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace EZSecCam
{
    public class Settings
    {
        public const string LOG_CONFIG = "Data/appsettings.json";

        public const string HAARCASCADE_FACES = "Data/haarcascade_frontalface_default.xml";
        public const string HAARCASCADE_EYES = "Data/haarcascade_eye_tree_eyeglasses.xml";
        public const string FACE_PICTURE = "Data/yalta.jpg";
        public const string FACE_PICTURE_PNG = "Data/lenna.png";

        public const string DEPLOY_PATH = "Data/deploy.prototxt";
        public const string DNN_MODEL = "Data/res10_300x300_ssd_iter_140000.caffemodel";

        public enum FilterType
        {
            None,
            FilterBrightness
        }

        public enum DetectorType
        {
            None,
            Haarcascade,
            DNN
        }

        public static FilterType filterType = FilterType.None;
        public static DetectorType detectorType = DetectorType.None;

        public static float Confidence = 0.6f;

        public static void UpdateBrightnessContrast(Mat src, Mat modifiedSrc, int brightness, int contrast)
        {
            brightness = brightness - 100;
            contrast = contrast - 100;

            double alpha, beta;
            if (contrast > 0)
            {
                double delta = 127f * contrast / 100f;
                alpha = 255f / (255f - delta * 2);
                beta = alpha * (brightness - delta);
            }
            else
            {
                double delta = -128f * contrast / 100;
                alpha = (256f - delta * 2) / 255f;
                beta = alpha * brightness + delta;
            }
            src.ConvertTo(modifiedSrc, MatType.CV_8UC3, alpha, beta);
        }

        public static Mat DetectFace(CascadeClassifier cascade, Mat frame)
        {
            Mat result;

            using (var gray = new Mat())
            {
                result = frame.Clone();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                // Detect faces
                Rect[] faces = cascade.DetectMultiScale(
                    gray, 1.08, 2, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(30, 30));

                // Render all detected faces
                foreach (Rect face in faces)
                {
                    var color = Scalar.FromRgb(255, 0, 0);

                    Cv2.Rectangle(result, face, color, 1);
                }
            }
            return result;
        }

        public static Mat DetectFaceDNN(Mat frame)
        {
            int frameHeight = frame.Rows;
            int frameWidth = frame.Cols;

            using var faceNet = CvDnn.ReadNetFromCaffe(DEPLOY_PATH, DNN_MODEL);
            using var blob = CvDnn.BlobFromImage(frame, 1.0, new Size(300, 300),
                new Scalar(104, 117, 123), false, false);
            faceNet.SetInput(blob, "data");

            using var detection = faceNet.Forward("detection_out");
            using var detectionMat = new Mat(detection.Size(2), detection.Size(3), MatType.CV_32F,
                detection.Ptr(0));
            for (int i = 0; i < detectionMat.Rows; i++)
            {
                float confidence = detectionMat.At<float>(i, 2);

                if (confidence > Confidence)
                {
                    int x1 = (int)(detectionMat.At<float>(i, 3) * frameWidth);
                    int y1 = (int)(detectionMat.At<float>(i, 4) * frameHeight);
                    int x2 = (int)(detectionMat.At<float>(i, 5) * frameWidth);
                    int y2 = (int)(detectionMat.At<float>(i, 6) * frameHeight);

                    Cv2.Rectangle(frame, new Point(x1, y1), new Point(x2, y2), new Scalar(0, 255, 0), 2,
                        LineTypes.Link4);
                    Cv2.PutText(frame,
                            "Face Confidence = "+(confidence*100).ToString("F1")+"%",
                            new Point(x2, y2),
                            HersheyFonts.HersheySimplex,
                            0.5f,
                            new Scalar(0, 255, 0),
                            2);
                }
            }
            return frame;
        }
    }
}
