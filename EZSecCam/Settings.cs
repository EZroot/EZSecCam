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
        public enum FilterType
        {
            None,
            FilterBrightness
        }

        public static FilterType filterType = FilterType.None;

         public const string HarrcascadePath = "Data/haarcascade_frontalface_default.xml";
         public const string FacepicturePath = "Data/yalta.jpg";
        public const string FacepicturePathPng = "Data/lenna.png";

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
                    var center = new Point
                    {
                        X = (int)(face.X + face.Width * 0.5),
                        Y = (int)(face.Y + face.Height * 0.5)
                    };
                    var axes = new OpenCvSharp.Size
                    {
                        Width = (int)(face.Width * 0.5),
                        Height = (int)(face.Height * 0.5)
                    };
                    Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
                }
            }
            return result;
        }
    }
}
