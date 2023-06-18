namespace Sas.Mathematica.Service.Converters
{
    public class ConvertAngle
    {
        public static double RadToDeg(double rad)
        {
            return 180 * rad / Constants.PI;
        }

        public static double DegToRad(double deg)
        {
            return Constants.PI * deg / 180.0;
        }
    }
}
