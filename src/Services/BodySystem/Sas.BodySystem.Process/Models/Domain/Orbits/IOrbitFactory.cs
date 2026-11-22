using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Process.Models.Domain.Orbits;

public interface IOrbitFactory
{
    IOrbit Create(string name, Vector relativePosition, Vector relativeVelocity,
        Vector gravitationalSourcePosition, double gravitationalParameter);
}
