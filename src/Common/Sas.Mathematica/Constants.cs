namespace Sas.Mathematica
{
    public class Constants
    {
        public static readonly double SolarMass = 1.9884 * Math.Pow(10, 30); // kg
        public static readonly double SunRadius = 1400000000; // m

        public static readonly double EarthMass = 5.97219 * Math.Pow(10, 24); // kg
        public static readonly double EarthPeriapsis = 147098291000; // m
        public static readonly double EarthApoapsis = 1.52098233 * Math.Pow(10,11); // m
        public static readonly double EarthMaxVelocity = 30290; // m
        public static readonly double EarthMinVelocity = 29290; // m

        public static readonly double EarthFlatness = 0.08181919;
        public static readonly double EarthRadius = 6378137; // m


        public static double MoonMass = 7.347673 * Math.Pow(10, 22); // kg
        public static readonly double MoonPeriapsis = 363228900; // m
        public static readonly double MoonApoapsis = 405696000; // m
        public static readonly double MoonMaxVelocity = 1082; // m
        public static readonly double MoonMinVelocity = 968; // m
        public static readonly double MoonRadius = 1737400; // m
        public static readonly double MoonMeanDistance = 1737400; // m
        public static readonly double MoonMeanVelocity = 1022; // m

        public const double G = 0.00000000006674301515; // m^3 kg^{-1} s^{-2}

    }
}