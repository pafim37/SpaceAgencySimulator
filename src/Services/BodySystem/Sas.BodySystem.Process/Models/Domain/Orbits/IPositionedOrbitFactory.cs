using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Process.Models.Domain.Orbits;

public interface IPositionedOrbitFactory
{
    IPositionedOrbit Create(string name, Vector relativePosition, Vector relativeVelocity,
        Vector gravitationalSourcePosition, double gravitationalParameter);
}
