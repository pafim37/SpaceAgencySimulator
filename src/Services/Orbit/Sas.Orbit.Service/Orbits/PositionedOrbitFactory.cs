using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Orbits;

internal sealed class PositionedOrbitFactory : IPositionedOrbitFactory
{
    public IPositionedOrbit Create(string name, Vector relativePosition, Vector relativeVelocity,
        Vector gravitationalSourcePosition, double gravitationalParameter)
    {
        OrbitElements elements = OrbitElementsFactory.GetOrbitElements(relativePosition, relativeVelocity, gravitationalParameter);
        return new PositionedOrbit(name, gravitationalSourcePosition, elements);
    }
}
