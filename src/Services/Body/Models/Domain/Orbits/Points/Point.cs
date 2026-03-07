namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public class Point
    {
        private double x;
        private double y;
        private double z;
        private readonly int precision = 13;

        public double X
        {
            get
            {
                return Math.Round(x, precision);
            }
            set
            {
                x = value;
            }
        }
        public double Y
        {
            get
            {
                return Math.Round(y, precision);
            }
            set
            {
                y = value;
            }
        }
        public double Z
        {
            get
            {
                return Math.Round(z, precision);
            }
            set
            {
                z = value;
            }
        }

        public Point(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        public static Point operator +(Point v1, Point v2)
        {
            return new Point(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
    }
}
