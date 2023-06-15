using Sas.Domain.Bodies.BodyExtensions;
using Sas.Domain.Models.Orbits;
using Sas.Domain.Models.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Bodies
{
    public class BodySystem
    {
        #region fields
        private const double TwoBodyProblemMassRatioLimit = 0.03;
        private const string BarycentrumName = "Barycentrum";
        private readonly List<Body> _bodies;
        private readonly List<OrbitHolder> _orbitsDescription;
        private readonly double _G; // gravitational constant
        #endregion

        #region properties
        /// <summary>
        /// Center of mass of the system (Barycentrum)
        /// </summary>
        public Body Barycentrum => GetBarycenter();

        /// <summary>
        /// G * (M + m1 + m2 + ...)
        /// </summary>
        public double U => GetU();

        /// <summary>
        /// G * (M + m1 + m2 + ...)
        /// </summary>
        public double G => _G;

        /// <summary>
        /// Returns list of bodies in current system
        /// </summary>
        public List<Body> Bodies => _bodies;

        /// <summary>
        /// Returns list of orbits in current system
        /// </summary>
        public List<OrbitHolder> OrbitsDescription => _orbitsDescription;
        #endregion

        #region public methods
        /// <summary>
        /// Adds a new body to the system
        /// </summary>
        /// <param name="body"></param>
        public void AddBody(Body body)
        {
            _bodies.Add(body);
            Update();
        }

        /// <summary>
        /// Update state of the Solar System
        /// </summary>
        public void Update()
        {
            FindOrbits();
        }
        #endregion

        #region constructors
        public BodySystem(IEnumerable<Body> bodies, double gravitationalConst = Constants.G)
        {
            _bodies = bodies.ToList();
            _orbitsDescription = new();
            _G = gravitationalConst;
            Update();
        }

        public BodySystem()
        {
            _bodies = new();
            _orbitsDescription = new();
        }
        #endregion

        #region private methods
        private void FindOrbits()
        {
            List<Body> sortBodies = _bodies.OrderBy(x => x.Mass).ToList();
            for (int i = 0; i < sortBodies.Count - 1; i++)
            {
                Body surroundedBody = sortBodies[i];
                Body? resultBody = null;
                double distance = double.MaxValue;
                for (int j = i + 1; j < sortBodies.Count; j++)
                {
                    Body centerBody = sortBodies[j];
                    {
                        double relativeDistance = surroundedBody.GetPositionRelatedTo(centerBody).Magnitude;
                        double influence = centerBody.GetSphereOfInfluenceRelatedTo(surroundedBody);
                        if (relativeDistance < distance && influence >= relativeDistance)
                        {
                            distance = relativeDistance;
                            resultBody = centerBody;
                        }
                    }
                }
                if (surroundedBody.Mass / resultBody.Mass < TwoBodyProblemMassRatioLimit)  // TODO: remove this hardcoded value
                {
                    AddBodyToSystem(surroundedBody, resultBody);
                }
                else
                {
                    AddBodyToSystem(sortBodies[^1], Barycentrum);
                }
            }
        }
        public void CalibrateBarycenterToZero()
        {
            Body barycenter = GetBarycenter();
            foreach (Body body in _bodies)
            {
                body.Position -= barycenter.Position;
            }
        }
        private void AddBodyToSystem(Body surroundedBody, Body? resultBody)
        {
            var orbit = OrbitFactory.CalculateOrbit(
                surroundedBody.GetPositionRelatedTo(resultBody),
                surroundedBody.GetVelocityRelatedTo(resultBody),
                _G * (surroundedBody.Mass + resultBody.Mass));
            Vector center = Vector.Zero;
            double rotationAngle = 0;
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                rotationAngle = orbit.RotationAngle.Value;
                center = new Vector(resultBody.Position.X - Math.Cos(rotationAngle) * orbit.Eccentricity * orbit.SemiMajorAxis.Value, Math.Sin(rotationAngle) * orbit.Eccentricity * orbit.SemiMajorAxis.Value + resultBody.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Circular)
            {
                center = new Vector(resultBody.Position.X, resultBody.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Hyperbolic)
            {
                rotationAngle = -orbit.RotationAngle.Value;
                center = new Vector(orbit.MinDistance - orbit.SemiMajorAxis.Value + Math.Cos(rotationAngle) * resultBody.Position.X, orbit.MinDistance - orbit.SemiMajorAxis.Value + resultBody.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Parabolic)
            {
                rotationAngle = orbit.RotationAngle.Value;
                center = new Vector(resultBody.Position.Y, resultBody.Position.X, 0);
            }
            var orbitHolder = new OrbitHolder()
            {
                Name = surroundedBody.Name,
                Orbit = orbit,
                Center = center,
                Rotation = rotationAngle
            };
            _orbitsDescription.Add(orbitHolder);
        }
        private Body GetBarycenter()
        {
            Vector position = Vector.Zero;
            foreach (Body body in _bodies)
            {
                position += body.Mass * body.Position;
            }
            double totalMass = _bodies.Sum(body => body.Mass);
            position = 1 / totalMass * position;
            return new Body(BarycentrumName, totalMass, position, Vector.Zero);
        }
        private double GetU()
        {
            double totalMass = _bodies.Sum(body => body.Mass);
            return totalMass * _G;
        }
        #endregion
    }
}
