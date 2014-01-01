using System.Collections.Generic;

namespace TDBCore.Types.libs
{

    public class RectangleD
    {
        double x = 0;
        double y = 0;
        double x_length = 0;
        double y_length = 0;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Width
        {
            get { return x_length; }
            set { x_length = value; }
        }

        public double Height
        {
            get { return y_length; }
            set { y_length = value; }
        }

        public RectangleD()
        {

        }

        public RectangleD(double _x, double _y, double _x_length, double _y_length)
        {
            x = _x;
            y = _y;
            x_length = _x_length;
            y_length = _y_length;

        }


        public RectangleD(double _x, double _y)
        {


            x = _x;
            y = _y;


        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            RectangleD newRecD = (RectangleD)obj;


            if (newRecD.X != this.X ||
                newRecD.Y != this.Y ||
                this.Width != newRecD.Width ||
                this.Height != newRecD.Height)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static string ToSquareCSV(IList<RectangleD> list)
        {
            string locationString = "";

            foreach (RectangleD recd in list)
            {
                locationString += "," + recd.x + "," + recd.y + "," + recd.Width;
            }

            if (locationString != "")
                locationString = locationString.Remove(0, 1);

            return locationString;

        }

    }


}
