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
    public class Contour
    {
        public Point[] Data { get; set; }
        public int[] Hierarchy { get; set; }

        public Contour(Point[] data)
        {
            this.Data = data;
        }

        public VectorOfPoint ToVectorOfPoint()
        {
            return new VectorOfPoint(this.Data);
        }

        public RotatedRect GetRotatedRect()
        {
            return new WRectangle(this).Rectangle;
        }

        public static Contour Largest(List<Contour> contours)
        {
            return contours.OrderByDescending(c => CvInvoke.ContourArea(new VectorOfPoint(c.Data))).FirstOrDefault();
        }

        public static Contour Nth(List<Contour> contours, int n)
        {
            return contours.OrderBy(c => CvInvoke.ContourArea(new VectorOfPoint(c.Data))).ElementAtOrDefault(n);
        }

        public static Contour Smallest(List<Contour> contours)
        {
            return contours.OrderBy(c => CvInvoke.ContourArea(new VectorOfPoint(c.Data))).FirstOrDefault();
        }

        public static void Draw(WImage img, Contour contour, Color color, int thickness = 1)
        {
            VectorOfVectorOfPoint data = new VectorOfVectorOfPoint(new VectorOfPoint(contour.Data));
            CvInvoke.DrawContours(img.Data, data, 0, new MCvScalar(color.B, color.G, color.R), thickness);
        }

        public static void Draw(WImage img, List<Contour> contours, Color color, int thickness = 1)
        {
            contours.ForEach(c => Draw(img, c, color, thickness));
        }

        public static VectorOfVectorOfPoint ToVectorOfVectorOfPoint(Point[][] points)
        {
            return new VectorOfVectorOfPoint(points);
        }
    }
}
