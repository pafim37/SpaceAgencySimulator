namespace Sas.Mathematica.Service.Converters
{
    public static class ConvertNumberToAngle
    {
        public static float ToHours(float number)
        {
            return number * 24;
        }

        public static float ToDegs(float number)
        {
            return number * 360;
        }

        public static double ToRads(float number)
        {
            return number * 2 * Constants.PI; 
        }
    }
}
