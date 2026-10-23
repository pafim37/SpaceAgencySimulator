namespace Sas.Body.Service.Models.Domain.Orbits.Points
{
    public readonly struct Point(double x, double y, double z)
    {
        private const int Precision = 13;

        public double X { get; } = Math.Round(x, Precision);
        public double Y { get; } = Math.Round(y, Precision);
        public double Z { get; } = Math.Round(z, Precision);
        public static Point operator +(Point v1, Point v2)
        => new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

    }
}
