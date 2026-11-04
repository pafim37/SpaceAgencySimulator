using Sas.Body.Core.DataTransferObject;

namespace Sas.Body.Business
{
    internal static class DefaultBodyFactory
    {
        public static IEnumerable<NewBodyDto> Create(List<string> names)
        {
            float sunMass = 332950;
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
            if (names.Contains("Mercury"))
            {
                double distance = 38;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Mercury",
                    Enabled = true,
                    Mass = 0.0552,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Wenus"))
            {
                double distance = 72;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Wenus",
                    Enabled = true,
                    Mass = 0.8149,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Earth"))
            {
                double distance = 100;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Earth",
                    Enabled = true,
                    Mass = 1,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Moon"))
            {
                double diff = 8;
                double distance = diff;
                double velocity = Math.Round(Math.Sqrt(1 / distance), 2); // relative to Earth
                yield return new NewBodyDto()
                {
                    Name = "Moon",
                    Enabled = true,
                    Mass = 0.01,
                    Radius = 0.5,
                    Position = new VectorDto { X = 100, Y = diff, Z = 0 }, // relative to Earth
                    Velocity = new VectorDto { X = -velocity, Y = -Math.Round(Math.Sqrt(sunMass / 100), 2), Z = 0 },
                };
            }
            if (names.Contains("Mars"))
            {
                double distance = 152;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Mars",
                    Enabled = true,
                    Mass = 1,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Player"))
            {
                double diff = 4;
                double distance = 100 + diff;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Player",
                    Enabled = true,
                    Mass = 0.01,
                    Radius = 0.001,
                    Position = new VectorDto { X = distance - diff, Y = diff, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
        }
    }
}
