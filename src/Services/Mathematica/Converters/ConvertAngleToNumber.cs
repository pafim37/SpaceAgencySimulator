namespace Sas.Mathematica.Service.Converters
{
    public static class ConvertAngleToNumber
    {
        public static float GetHour(int hh, int mm, float ss)
        {
            float hours = hh + (mm + ss / 60) / 60;
            return GetHour(hours);
        }

        public static float GetHour(float hh)
        {
            return hh / 24;
        }

        public static float GetDeg(int deg, int mm, float ss)
        {
            float degs = deg + (mm + ss / 60) / 60;
            return GetDeg(degs);
        }

        public static float GetDeg(float deg)
        {
            return deg / 360;
        }

        public static float GetRad(float rad)
        {
            return rad;
        }
    }
}
