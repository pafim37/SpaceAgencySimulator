using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;
namespace Sas.Orbit.Service.Orbits
{
    public class TwoBodyProblemOrbit
    {

        public static PositionedOrbit GetOrbit(Vector positionRel, Vector velocityRel, double gravitationalParameter)
        {
            IOrbitElements orbitElements = OrbitElementsFactory.GetOrbitElements(positionRel, velocityRel, gravitationalParameter);
            PositionedOrbit positionedOrbit = new PositionedOrbit("Two-Body Orbit", Vector.Zero, orbitElements);
            return positionedOrbit;
        }
    }
}
