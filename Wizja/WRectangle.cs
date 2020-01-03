using Emgu.CV;
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
    public class WRectangle
    {
        public Point[] Data { get; set; }
        public RotatedRect Rectangle { get; set; }

        public WRectangle(Contour contour)
        {
            var points = new VectorOfPoint(contour.Data);

            this.Rectangle = CvInvoke.MinAreaRect(points);
            this.Data = this.Rectangle.GetVertices().Select(v => Point.Round(v)).ToArray();
        }

        public double ToRightAngle()
        {
            if (this.Rectangle.Angle < -45)
            {
                return -(this.Rectangle.Angle + 90);
            }
            else
            {
                return -(this.Rectangle.Angle);
            }
        }

        public static void Draw(WImage img, WRectangle rectangle, Color color, int thickness = 1)
        {
            VectorOfVectorOfPoint data = new VectorOfVectorOfPoint(new VectorOfPoint(rectangle.Data));
            CvInvoke.DrawContours(img.Data, data, 0, new MCvScalar(color.B, color.G, color.R), thickness);
        }

    }
}
