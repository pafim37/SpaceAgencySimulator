using Sas.Mathematica.Service.Vectors;

namespace Sas.Orbit.Service.Orbits;

public interface IPositionedOrbitFactory
{
    IPositionedOrbit Create(string name, Vector relativePosition, Vector relativeVelocity,
        Vector gravitationalSourcePosition, double gravitationalParameter);
}
