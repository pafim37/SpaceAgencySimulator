namespace Sas.BodySystem.Process.Models.Domain.Orbits;

/// <summary>Orbit data returned by Orbit service, without a dependency on its implementation.</summary>
public sealed class OrbitData
{
    public required string Name { get; init; }
    public required List<OrbitPointData> Points { get; init; }
}