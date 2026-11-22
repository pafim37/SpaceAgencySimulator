using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.BodySystem.Process.Models.Domain.Orbits;

public sealed class OrbitFactory : IOrbitFactory
{
    public IOrbit Create(string name, Vector relativePosition, Vector relativeVelocity,
        Vector gravitationalSourcePosition, double gravitationalParameter)
    {
        OrbitElements elements = OrbitElementsFactory.GetOrbitElements(relativePosition, relativeVelocity, gravitationalParameter);
        return new Orbit(name, gravitationalSourcePosition, elements);
    }
}
