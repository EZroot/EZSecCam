using OpenCvSharp;
using OpenCvSharp.Dnn;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace EZSecCam
{
    public class Settings
    {
        private Settings()
        { 
        }

        public const string LOG_CONFIG = "Data/appsettings.json";

        public const string HAARCASCADE_FACES = "Data/haarcascade_frontalface_default.xml";
        public const string HAARCASCADE_EYES = "Data/haarcascade_eye_tree_eyeglasses.xml";
        public const string FACE_PICTURE = "Data/yalta.jpg";
        public const string FACE_PICTURE_PNG = "Data/lenna.png";

        public const string DEPLOY_PATH = "Data/deploy.prototxt";
        public const string DNN_MODEL = "Data/res10_300x300_ssd_iter_140000.caffemodel";

        public const string YOLO_CFG_PATH = "Data/yolov3.cfg";
        public const string YOLO_WEIGHT_PATH = "Data/yolov3.weights";

        public const string YOLOTINY_CFG_PATH = "Data/yolov3-tiny.cfg";
        public const string YOLOTINY_WEIGHT_PATH = "Data/yolov3-tiny.weights";

        public const string COCO_NAMES_PATH = "Data/coco.names";

        const int PREFIX = 5;   //skip 0~4

        public enum FilterType
        {
            None,
            FilterBrightness
        }

        public enum DetectorType
        {
            None,
            Haarcascade,
            Caffe,
            YoloV3
        }

        public static FilterType filterType = FilterType.None;
        public static DetectorType detectorType = DetectorType.None;

        public static float Confidence = 0.6f;
        public static float NmsConfidence = 0.3f;

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

        private static readonly Scalar[] Colors = Enumerable.Repeat(false, 80).Select(x => Scalar.RandomColor()).ToArray();
        private static readonly string[] Labels = File.ReadAllLines(COCO_NAMES_PATH).ToArray();

        static Net faceNet;
        static Mat blob;

        public static void Init()
        {
            //faceNet = CvDnn.ReadNetFromDarknet(YOLOTINY_CFG_PATH, YOLOTINY_WEIGHT_PATH);
            faceNet = CvDnn.ReadNetFromDarknet(YOLO_CFG_PATH, YOLO_WEIGHT_PATH);
            faceNet.SetPreferableBackend(Net.Backend.OPENCV);
            /*
            0:DNN_BACKEND_DEFAULT 
            1:DNN_BACKEND_HALIDE 
            2:DNN_BACKEND_INFERENCE_ENGINE
            3:DNN_BACKEND_OPENCV 
             */
            faceNet.SetPreferableTarget(0);

            /*
            0:DNN_TARGET_CPU 
            1:DNN_TARGET_OPENCL
            2:DNN_TARGET_OPENCL_FP16
            3:DNN_TARGET_MYRIAD 
            4:DNN_TARGET_FPGA 
             */
        }

        public static Mat DetectFaceYoloV3(Mat frame, bool nms=true)
        {
            //facenet readnetfromdarknet
            var blob = CvDnn.BlobFromImage(frame, 1 / 255.0, new Size(544, 544), new Scalar(), true, false);
            //var faceNet = CvDnn.ReadNetFromDarknet(YOLOTINY_CFG_PATH, YOLOTINY_WEIGHT_PATH);
            //faceNet.SetPreferableBackend(Net.Backend.OPENCV);
            /*
            0:DNN_BACKEND_DEFAULT 
            1:DNN_BACKEND_HALIDE 
            2:DNN_BACKEND_INFERENCE_ENGINE
            3:DNN_BACKEND_OPENCV 
             */
            //faceNet.SetPreferableTarget(0);
            
            /*
            0:DNN_TARGET_CPU 
            1:DNN_TARGET_OPENCL
            2:DNN_TARGET_OPENCL_FP16
            3:DNN_TARGET_MYRIAD 
            4:DNN_TARGET_FPGA 
             */
            faceNet.SetInput(blob);

            //get output layer name
            var outNames = faceNet.GetUnconnectedOutLayersNames();

            //create mats for output layer
            Mat[] outs = outNames.Select(_ => new Mat()).ToArray();

            #region forward model
            Stopwatch sw = new Stopwatch();
            sw.Start();

            faceNet.Forward(outs, outNames);

            sw.Stop();
            Log.Debug("Runtime:{0} ms", sw.ElapsedMilliseconds);
            #endregion

            //get result
            //for nms
            var classIds = new List<int>();
            var confidences = new List<float>();
            var probabilities = new List<float>();
            var boxes = new List<Rect2d>();

            var w = frame.Width;
            var h = frame.Height;
            /*
             YOLO3 COCO trainval output
             0 1 : center                    2 3 : w/h
             4 : confidence                  5 ~ 84 : class probability 
            */

            foreach (var prob in outs)
            {
                for (var i = 0; i < prob.Rows; i++)
                {
                    var confidence = prob.At<float>(i, 4);
                    if (confidence > Confidence)
                    {
                        //get classes probability
                        Cv2.MinMaxLoc(prob.Row(i).ColRange(PREFIX, prob.Cols), out _, out Point max);
                        var classes = max.X;
                        var probability = prob.At<float>(i, classes + PREFIX);

                        if (probability > Confidence) //more accuracy, you can cancel it
                        {
                            //get center and width/height
                            var centerX = prob.At<float>(i, 0) * w;
                            var centerY = prob.At<float>(i, 1) * h;
                            var width = prob.At<float>(i, 2) * w;
                            var height = prob.At<float>(i, 3) * h;

                            if (!nms)
                            {
                                // draw result (if don't use NMSBoxes)
                                Draw(frame, classes, confidence, probability, centerX, centerY, width, height);
                                continue;
                            }

                            //put data to list for NMSBoxes
                            classIds.Add(classes);
                            confidences.Add(confidence);
                            probabilities.Add(probability);
                            boxes.Add(new Rect2d(centerX, centerY, width, height));
                        }
                    }
                }
            }

            if (!nms) return frame;

            //using non-maximum suppression to reduce overlapping low confidence box
            CvDnn.NMSBoxes(boxes, confidences, Confidence, NmsConfidence, out int[] indices);

            foreach (var i in indices)
            {
                var box = boxes[i];
                Draw(frame, classIds[i], confidences[i], probabilities[i], box.X, box.Y, box.Width, box.Height);
            }

            return frame;
        }

        public static Mat DetectFace(CascadeClassifier cascade, Mat frame)
        {
            Mat result;

            using (var gray = new Mat())
            {
                result = frame.Clone();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                // Detect faces
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Rect[] faces = cascade.DetectMultiScale(
                    gray, 1.08, 2, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(30, 30));
                sw.Stop();
                Log.Debug("Runtime:{0} ms", sw.ElapsedMilliseconds);

                // Render all detected faces
                foreach (Rect face in faces)
                {
                    var color = Scalar.FromRgb(255, 0, 0);

                    Cv2.Rectangle(result, face, color, 1);
                }
            }
            return result;
        }

        public static Mat DetectFaceCaffe(Mat frame)
        {
            int frameHeight = frame.Rows;
            int frameWidth = frame.Cols;

            using var faceNet = CvDnn.ReadNetFromCaffe(DEPLOY_PATH, DNN_MODEL);
            using var blob = CvDnn.BlobFromImage(frame, 1.0, new Size(300, 300),
                new Scalar(104, 117, 123), false, false);
            faceNet.SetInput(blob, "data");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            using var detection = faceNet.Forward("detection_out");
            sw.Stop();
            Log.Debug("Runtime:{0} ms", sw.ElapsedMilliseconds);

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

                    Cv2.Rectangle(frame, new Point(x1, y1), new Point(x2, y2), new Scalar(255, 0, 0), 2,
                        LineTypes.Link4);
                    Cv2.PutText(frame,
                            "Confidence " + (confidence * 100).ToString("F1") + "%",
                            new Point(x2, y2),
                            HersheyFonts.HersheySimplex,
                            .8f,
                            new Scalar(0, 255, 0),
                            1,
                            LineTypes.AntiAlias);
                }
            }
            return frame;
        }

        private static void Draw(Mat image, int classes, float confidence, float probability, double centerX, double centerY, double width, double height)
        {
            //label formating
            var label = $"{Labels[classes]} {probability * 100:0.00}%";
            Log.Debug("confidence {0} % - {1}", confidence * 100, label);
            var x1 = (centerX - width / 2) < 0 ? 0 : centerX - width / 2; //avoid left side over edge
            //draw result
            image.Rectangle(new Point(x1, centerY - height / 2), new Point(centerX + width / 2, centerY + height / 2), Colors[classes], 2);
            var textSize = Cv2.GetTextSize(label, HersheyFonts.HersheyTriplex, 0.5, 1, out var baseline);
            Cv2.Rectangle(image, new Rect(new Point(x1, centerY - height / 2 - textSize.Height - baseline),
                new Size(textSize.Width, textSize.Height + baseline)), Colors[classes], Cv2.FILLED);
            var textColor = Cv2.Mean(Colors[classes]).Val0 < 70 ? Scalar.White : Scalar.Black;
            Cv2.PutText(image, label, new Point(x1, centerY - height / 2 - baseline), HersheyFonts.HersheyTriplex, 0.5, textColor);
        }
    }
}
