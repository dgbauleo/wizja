using Emgu.CV;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wizja
{
    public static class Extension
    {
        public static Contour Smallest(this List<Contour> contours)
        {
            return contours.OrderBy(c => CvInvoke.ContourArea(new VectorOfPoint(c.Data))).FirstOrDefault();
        }

        public static Contour Largest(this List<Contour> contours)
        {
            return contours.OrderByDescending(c => CvInvoke.ContourArea(new VectorOfPoint(c.Data))).FirstOrDefault();
        }

        public static Contour Nth(this List<Contour> contours, int n)
        {
            return contours.OrderBy(c => CvInvoke.ContourArea(new VectorOfPoint(c.Data))).ElementAtOrDefault(n);
        }

        public static List<Contour> Triangles(this List<Contour> contours)
        {
            return contours.WithEdges(3);
        }

        public static List<Contour> Rectangles(this List<Contour> contours)
        {
            return contours.WithEdges(4);
        }

        public static List<Contour> WithEdges(this List<Contour> contours, int edges)
        {
            return contours.Where(c => new VectorOfPoint(c.Data).Size == edges).ToList();
        }
    }
}
