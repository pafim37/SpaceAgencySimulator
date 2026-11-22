using Sas.Orbit.Service.Orbits.Primitives;
using System.Text.Json.Serialization;

namespace Sas.BodySystem.Process.Models.Domain.Orbits;

[JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
public sealed class OrbitElementsDto
{
    public OrbitType OrbitType { get; init; }
    public double? SemiMajorAxis { get; init; }
    public double? SemiMinorAxis { get; init; }
    public double Eccentricity { get; init; }
    public double ArgumentOfPeriapsis { get; init; }
    public double Inclination { get; init; }
    public double AscendingNode { get; init; }
    public double TrueAnomaly { get; init; }
    public double EccentricAnomaly { get; init; }
    public double MeanAnomaly { get; init; }
    public double? Period { get; init; }
    public double? Radius { get; init; }
    public double GravitationalParameter { get; init; }
    public double FirstNode { get; init; }
    public double MeanMotion { get; init; }
}
