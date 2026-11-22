namespace Sas.BodySystem.Process.Models.Domain.Orbits;

/// <summary>Orbit data returned by Orbit service, without a dependency on its implementation.</summary>
public sealed class OrbitDto
{
    public OrbitDto()
    {
        // Default constructor for serialization
    }
    public required string Name { get; init; }
    public required OrbitElementsDto OrbitElements { get; init; }
    public required List<OrbitPointDto> Points { get; init; }
}
