using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using Point = OpenCvSharp.Point;

namespace EZSecCam
{
    public class Settings
    {
        public const string HAARCASCADE_FACES = "Data/haarcascade_frontalface_default.xml";
        public const string HAARCASCADE_EYES = "Data/haarcascade_eye_tree_eyeglasses.xml";
        public const string FACE_PICTURE = "Data/yalta.jpg";
        public const string FACE_PICTURE_PNG = "Data/lenna.png";
        public const string FACE_MODEL = "Data/res10_300x300_ssd_iter_140000_fp16.caffemodel";

        public enum FilterType
        {
            None,
            FilterBrightness
        }

        public enum DetectorType
        {
            None,
            Haarcascade
        }

        public static FilterType filterType = FilterType.None;
        public static DetectorType detectorType = DetectorType.None;

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
    }
}
