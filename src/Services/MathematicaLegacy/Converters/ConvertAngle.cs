namespace Sas.MathematicaLegacy.Service.Converters
{
    [Obsolete("Legacy implementation. Use Sas.Mathematica.Service instead.")]
    public class ConvertAngle
    {
        [Obsolete("Legacy implementation. Use Sas.Mathematica.Service instead.")]
        public static double RadToDeg(double rad)
        {
            return 180 * rad / Constants.PI;
        }

        [Obsolete("Legacy implementation. Use Sas.Mathematica.Service instead.")]
        public static double DegToRad(double deg)
        {
            return Constants.PI * deg / 180.0;
        }
    }
}
