using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TDBCore.Types
{
    public class DoublePoint
    {
        public double percentage;
        public double width;

        public double x;

        public double mouse_x;

        public double movePerc;
        public double mouseXPercLocat;

        public double working;
        public double working2;

        public double drawingX1;

        public double drawingX2;

        public double middrawingX1;

        public double middrawingX2;

        public DoublePoint(double _x, double _percentage, double _width, double _mouse_x,
            double movePerc, double _mouseXPercLocat, double _working, double _working2, double _drawingX1, double _drawingX2, double _middrawingX1, double _middrawingX2)
        {
            this.x = _x;
            this.percentage = _percentage;
            this.mouse_x = _mouse_x;
            this.width = _width;
            this.movePerc = movePerc;
            this.mouseXPercLocat = _mouseXPercLocat;
            this.working = _working;
            this.working2 = _working2;
            middrawingX1 = _middrawingX1;
            middrawingX2 = _middrawingX2;
            this.drawingX1 = _drawingX1;
            this.drawingX2 = _drawingX2;
        }

        public static void GenerateCSV(List<DoublePoint> _points)
        {

            Debug.WriteLine("percentage,width,x,mouse_x,movePerc,mouseXPercLocat,dx1,dx2,middx1,middx2, initdx1, initdx2");

            foreach (DoublePoint point in _points)
            {
                Debug.WriteLine(point.percentage.ToString() + "," +
                     point.width.ToString() + "," +
                     point.x.ToString("") + "," +
                     point.mouse_x.ToString("") + "," +
                     point.movePerc.ToString("") + "," +
                     point.mouseXPercLocat.ToString("") + "," +
                     point.drawingX1.ToString("") + "," +
                     point.drawingX2.ToString("") + "," +
                     point.middrawingX1.ToString("") + "," +
                     point.middrawingX2.ToString("") + "," +
                     point.working.ToString("") + "," +
                     point.working2.ToString(""));

            }


        }
    }
}
