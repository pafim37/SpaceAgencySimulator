using Sas.Body.Service.Models.Domain.BodyExtensions;
using Sas.Body.Service.Models.Domain.Orbits;
using Sas.Body.Service.Models.Domain.Orbits.Primitives;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain
{
    public class BodySystem
    {
        #region fields
        private const double TwoBodyProblemMassRatioLimit = 0.03;
        private const string BarycentrumName = "Barycentrum";
        private readonly List<BodyDomain> _bodies;
        private readonly List<OrbitHolder> _orbitsDescription;
        private readonly double _G; // gravitational constant
        #endregion

        #region properties
        /// <summary>
        /// Center of mass of the system (Barycentrum)
        /// </summary>
        public BodyDomain Barycentrum => GetBarycenter();

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
        public List<BodyDomain> Bodies => _bodies;

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
        public void AddBody(BodyDomain body)
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
        public BodySystem(IEnumerable<BodyDomain> bodies, double gravitationalConst = Constants.G)
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
            List<BodyDomain> sortBodies = _bodies.OrderBy(x => x.Mass).ToList();
            for (int i = 0; i < sortBodies.Count - 1; i++)
            {
                BodyDomain currentSurroundedBody = sortBodies[i];
                BodyDomain centerBody = new();
                double distance = double.MaxValue;
                for (int j = i + 1; j < sortBodies.Count; j++)
                {
                    BodyDomain currentCenterBody = sortBodies[j];
                    {
                        double relativeDistance = currentSurroundedBody.GetPositionRelatedTo(currentCenterBody).Magnitude;
                        double influence = currentCenterBody.GetSphereOfInfluenceRelatedTo(currentSurroundedBody);
                        if (relativeDistance < distance && influence >= relativeDistance)
                        {
                            distance = relativeDistance;
                            centerBody = currentCenterBody;
                        }
                    }
                }
                if (currentSurroundedBody.Mass / centerBody.Mass < TwoBodyProblemMassRatioLimit)
                {
                    AddOrbitToSystem(currentSurroundedBody, centerBody);
                }
                else
                {
                    AddOrbitToSystem(sortBodies[^1], Barycentrum);
                }
            }
        }
        public void CalibrateBarycenterToZero()
        {
            BodyDomain barycenter = GetBarycenter();
            foreach (BodyDomain body in _bodies)
            {
                body.Position -= barycenter.Position;
            }
        }
        private void AddOrbitToSystem(BodyDomain surroundedBody, BodyDomain resultBody)
        {
            Vector position = surroundedBody.GetPositionRelatedTo(resultBody);
            Vector velocity = surroundedBody.GetVelocityRelatedTo(resultBody);
            double u = _G * (surroundedBody.Mass + resultBody.Mass);
            Orbit orbit = OrbitFactory.CalculateOrbit(position, velocity, u);
            Vector center = Vector.Zero;
            double rotationAngle = 0;
            if (orbit.OrbitType == OrbitType.Elliptic)
            {
                rotationAngle = orbit.RotationAngle;
                center = new Vector(resultBody.Position.X - Math.Cos(rotationAngle) * orbit.Eccentricity * orbit.SemiMajorAxis!.Value, Math.Sin(rotationAngle) * orbit.Eccentricity * orbit.SemiMajorAxis.Value + resultBody.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Circular)
            {
                center = new Vector(resultBody.Position.X, resultBody.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Hyperbolic)
            {
                rotationAngle = -orbit.RotationAngle;
                center = new Vector(orbit.MinDistance - orbit.SemiMajorAxis!.Value + Math.Cos(rotationAngle) * resultBody.Position.X, orbit.MinDistance - orbit.SemiMajorAxis.Value + resultBody.Position.Y, 0);
            }
            else if (orbit.OrbitType == OrbitType.Parabolic)
            {
                rotationAngle = orbit.RotationAngle;
                center = new Vector(resultBody.Position.Y, resultBody.Position.X, 0);
            }
            OrbitHolder orbitHolder = new()
            {
                Name = surroundedBody.Name,
                Orbit = orbit,
                Center = center,
                Rotation = rotationAngle
            };
            _orbitsDescription.Add(orbitHolder);
        }
        private BodyDomain GetBarycenter()
        {
            Vector position = Vector.Zero;
            foreach (BodyDomain body in _bodies)
            {
                position += body.Mass * body.Position;
            }
            double totalMass = _bodies.Sum(body => body.Mass);
            position = 1 / totalMass * position;
            return new BodyDomain(BarycentrumName, totalMass, position, Vector.Zero);
        }
        private double GetU()
        {
            double totalMass = _bodies.Sum(body => body.Mass);
            return totalMass * _G;
        }
        #endregion
    }
}
