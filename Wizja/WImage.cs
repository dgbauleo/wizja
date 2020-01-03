using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wizja
{
    public class WImage
    {
        public Mat Data { get; set; }
        
        public WImage(string path, ImreadModes mode = ImreadModes.Color)
        {
            this.Data = CvInvoke.Imread(path, mode);
        }

        public void Threshold(ThresholdType type = ThresholdType.Binary, double threshold = 30, double maxValue = 255)
        {
            CvInvoke.Threshold(this.Data, this.Data, threshold, maxValue, type);
        }

        public void AdaptiveThreshold(AdaptiveThresholdType adaptiveType = AdaptiveThresholdType.MeanC, ThresholdType thresholdType = ThresholdType.Binary, double maxValue = 255)
        {
            CvInvoke.AdaptiveThreshold(this.Data, this.Data, maxValue, adaptiveType, thresholdType, 201, -2);
        }

        public List<Contour> Polygons(ChainApproxMethod approximationMethod = ChainApproxMethod.ChainApproxSimple)
        {
            List<Contour> output = new List<Contour>();

            List<Contour> contours = this.Contours(RetrType.Ccomp, approximationMethod);

            VectorOfPoint polygon = new VectorOfPoint();

            foreach (Contour contour in contours)
            {
                if (contour.Hierarchy[2] < 0)
                {
                    continue;
                }

                VectorOfPoint contourVector = new VectorOfPoint(contour.Data);
                CvInvoke.ApproxPolyDP(contourVector, polygon, CvInvoke.ArcLength(contourVector, true) * 0.05, true);

                output.Add(new Contour(polygon.ToArray()));
            }

            return output;
        }

        public List<Contour> Contours(RetrType retrievalMode = RetrType.Tree, ChainApproxMethod approximationMethod = ChainApproxMethod.ChainApproxSimple)
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            
            CvInvoke.FindContours(this.Data, contours, hierarchy, retrievalMode, approximationMethod);

            Point[][] contourArray = contours.ToArrayOfArray();
            int[,,] hierarchyArray = (int[,,])hierarchy.GetData();

            List<Contour> output = new List<Contour>();
            for (int i = 0; i < contourArray.Length; i++)
            {
                Contour c = new Contour(contourArray[i])
                {
                    Hierarchy = new int[] {
                        hierarchyArray[0, i, 0],
                        hierarchyArray[0, i, 1],
                        hierarchyArray[0, i, 2],
                        hierarchyArray[0, i, 3],
                    }
                };

                output.Add(c);
            }

            return output;
        }

        public void GaussianBlur(int intensity)
        {
            CvInvoke.GaussianBlur(this.Data, this.Data, new Size(intensity, intensity), 5);
        }

        public void FindEdges(double t1 = 180, double t2 = 120)
        {
            CvInvoke.Canny(this.Data, this.Data, t1, t2);
        }

        public void Crop(Size size, PointF center)
        {
            CvInvoke.GetRectSubPix(this.Data, size, center, this.Data);
        }

        public void Morph(MorphOp op, Size size)
        {
            Mat hStruct = CvInvoke.GetStructuringElement(ElementShape.Rectangle, size, new Point());
            CvInvoke.MorphologyEx(this.Data, this.Data, op, hStruct, new Point(), 1, BorderType.Default, new MCvScalar());
        }

        public void Convert(ColorConversion color)
        {
            CvInvoke.CvtColor(this.Data, this.Data, color);
        }

        public void Resize(int width, int height)
        {
            CvInvoke.Resize(this.Data, this.Data, new Size(width, height));
        }

        public void Preview()
        {
            CvInvoke.Imshow(string.Format("Wizja Preview [{0}]", DateTime.Now.ToString()), this.Data);
            CvInvoke.WaitKey();
        }

        public void Save()
        {
            CvInvoke.Imwrite("wizja_output.jpg", this.Data);
        }
    }
}
