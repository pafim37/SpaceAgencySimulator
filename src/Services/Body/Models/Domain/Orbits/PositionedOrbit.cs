using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.Orbits
{
    public class PositionedOrbit : IPositionedOrbit
    {
        public IOrbitDescription OrbitDescription { get; init; }
        public string Name { get; init; }
        public double Phi { get; init; }
        public double Theta { get; init; }
        public double Eta { get; init; }
        public Vector? Center { get; set; }
        public List<Point>? Points { get; set; }

        public PositionedOrbit(string name, Vector velocity, IOrbitDescription orbitDescription)
        {
            Vector eVector = orbitDescription.EccentricityVector;
            ReferenceSystem rs = new(eVector);
            Vector v1 = Rotation.Rotate(Vector.Oy, Vector.Oz, rs.Phi);
            Vector v2 = Rotation.Rotate(v1, Vector.Oy, rs.Theta);

            OrbitDescription = orbitDescription;
            Name = name;
            Phi = rs.Phi;
            Theta = rs.Theta;
            Eta = Math.Atan2(Vector.DotProduct(velocity.CrossProduct(v2), eVector), Vector.DotProduct(velocity, v2));
        }
    }
}
