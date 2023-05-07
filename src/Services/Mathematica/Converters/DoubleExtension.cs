namespace Sas.Mathematica.Service.Converters
{
    public static class DoubleExtension
    {
        public static double Round(this double n, int digits = 8)
        {
            return Math.Round(n, digits);
        }
    }
}