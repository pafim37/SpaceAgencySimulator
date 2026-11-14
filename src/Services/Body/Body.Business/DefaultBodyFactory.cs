using Sas.Body.Core.DataTransferObject;
using Sas.Mathematica.Service;

namespace Sas.Body.Business
{
    internal static class DefaultBodyFactory
    {
        public static IEnumerable<NewBodyDto> Create(List<string> names)
        {
            double sunMass = Constants.SolarMass;
            if (names.Contains("Sun"))
            {
                yield return new NewBodyDto()
                {
                    Name = "Sun",
                    Enabled = true,
                    Mass = sunMass,
                    Radius = 5,
                    Position = new VectorDto { X = 0, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = 0, Z = 0 },
                };
            }
            if (names.Contains("Earth"))
            {
                double distance = Constants.EarthPeriapsis; // It has influence on Moon
                double velocity = Constants.EarthMaxVelocity;
                yield return new NewBodyDto()
                {
                    Name = "Earth",
                    Enabled = true,
                    Mass = Constants.EarthMass,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Moon"))
            {
                yield return new NewBodyDto()
                {
                    Name = "Moon",
                    Enabled = true,
                    Mass = Constants.MoonMass,
                    Radius = 0.5,
                    Position = new VectorDto { X = Constants.EarthPeriapsis, Y = Constants.MoonPeriapsis, Z = 0 }, // relative to Earth
                    Velocity = new VectorDto { X = Constants.MoonMaxVelocity, Y = Constants.EarthMaxVelocity, Z = 0 },
                };
            }
        }
    }
}
