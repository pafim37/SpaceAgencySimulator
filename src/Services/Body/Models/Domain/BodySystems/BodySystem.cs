using Sas.Body.Service.Extensions.BodyExtensions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.BodySystems
{
    public class BodySystem
    {
        #region fields
        private readonly List<BodyDomain> bodies;
        private readonly List<PositionedOrbit> orbits;
        private readonly double g; // gravitational constant
        private const string BarycentrumName = "Barycentrum";
        private BodyDomain? barycenter;
        #endregion

        #region properties
        /// <summary>
        /// Gets gravitational constant.
        /// </summary>
        public double G => g;

        /// <summary>
        /// Gets center of mass of the system (Barycentrum).
        /// </summary>
        public BodyDomain? Barycentrum => barycenter;

        /// <summary>
        /// Gets list of bodies in current body system.
        /// </summary>
        public List<BodyDomain> Bodies => bodies;

        /// <summary>
        /// Gets list of orbits in current body system.
        /// </summary>
        public List<PositionedOrbit> Orbits => orbits;
        #endregion

        #region constructors
        public BodySystem(IEnumerable<BodyDomain> bodies, double gravitationalConst = Constants.G)
        {
            this.bodies = bodies.ToList();
            orbits = [];
            g = gravitationalConst;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Adds a new body to the system.
        /// </summary>
        /// <param name="body"></param>
        public void AddBody(BodyDomain body)
        {
            bodies.Add(body);
        }

        /// <summary>
        /// It finds barycenter, establishes hierarchy between bodies and finds orbits. 
        /// </summary>
        public void UpdateBodySystem()
        {
            barycenter = FindBarycenter();
            EstablishHierarchy();
            FindOrbits();
        }

        /// <summary>
        /// It scale the body system size (orbits are preserved) 
        /// </summary>
        public void ScaleBodySystem() => bodies.ForEach(body => ScaleBody.Scale(body));

        /// <summary>
        /// It updates body system, calibrates barycenter to zero and caluculates orbits points
        /// </summary>
        public void FullUpdate()
        {
            ScaleBodySystem();
            UpdateBodySystem();
            CalibrateBarycenterToZero();
            AssignOrbitPoints();
        }

        /// <summary>
        /// Finds parents of the bodies.
        /// </summary>
        public void EstablishHierarchy()
        {
            List<BodyDomain> sortBodies = [.. bodies.OrderByDescending(x => x.Mass)]; // Sun, Earth, Moon, Satellite...
            if (sortBodies.Count == 0)
            {
                return;
            }
            BodyDomain mostMassiveBody = sortBodies[0];
            if (sortBodies.Count > 1)
            {
                BodyDomain planet = sortBodies[1];
                planet.ParentName = mostMassiveBody.Name;
                planet.SphereOfInfluenceRadius = planet.GetSphereOfInfluenceRelatedTo(mostMassiveBody);
            }
            for (int i = 2; i < sortBodies.Count; i++)
            {
                BodyDomain body = sortBodies[i];
                BodyDomain parentBody = mostMassiveBody;
                double tmpDistance = double.MaxValue;
                for (int j = 1; j < i; j++)
                {
                    BodyDomain other = sortBodies[j];
                    double relativeDistance = body.GetPositionRelatedTo(other).Magnitude;
                    BodyDomain parentOtherBody = sortBodies.Find(b => b.Name == other.ParentName) ?? sortBodies.First();
                    double sphereOfInfluenceOfParentOtherBodyRadius = other.GetSphereOfInfluenceRelatedTo(parentOtherBody);
                    if (relativeDistance < tmpDistance && sphereOfInfluenceOfParentOtherBodyRadius >= relativeDistance)
                    {
                        tmpDistance = relativeDistance;
                        parentBody = other;
                    }
                }
                body.ParentName = parentBody.Name;
                body.SphereOfInfluenceRadius = body.GetSphereOfInfluenceRelatedTo(parentBody);
            }
        }

        /// <summary>
        /// Finds orbits of the bodies.
        /// </summary>
        public void FindOrbits()
        {
            foreach (BodyDomain body in bodies)
            {
                if (body.ParentName is null) continue;
                BodyDomain? other = bodies.FirstOrDefault(b => b.Name == body.ParentName);
                if (other is null) continue;
                try
                {
                    PositionedOrbit orbit = OrbitFactory.GetOrbit(body, other, G);
                    orbits.Add(orbit);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Calibrate barycenter to zero. It changes position of bodies and orbits.
        /// </summary>
        public void CalibrateBarycenterToZero()
        {
            if (barycenter != null)
            {
                foreach (BodyDomain body in bodies)
                {
                    body.Position -= barycenter.Position;
                    body.Velocity -= barycenter.Velocity;
                }
                foreach (PositionedOrbit orbit in orbits)
                {
                    if (orbit.Center is not null)
                    {
                        orbit.Center -= barycenter.Position;
                    }
                }
                barycenter.Position = Vector.Zero;
                barycenter.Velocity = Vector.Zero;
            }
        }
        #endregion

        public void AssignOrbitPoints() => orbits.ForEach(orbit => orbit.Points = GetOrbitPointsFactory.GetPoints(orbit));

        #region private methods
        private BodyDomain? FindBarycenter()
        {
            if (bodies.Count > 1)
            {
                double maxMass = bodies.Max(b => b.Mass);
                IEnumerable<BodyDomain> mostMassiveBodies = bodies.Where(b => b.Mass == maxMass);
                Vector position = Vector.Zero;
                Vector velocity = Vector.Zero;
                foreach (BodyDomain body in mostMassiveBodies)
                {
                    velocity += body.Velocity;
                }
                foreach (BodyDomain body in bodies)
                {
                    position += body.Mass * body.Position;
                }
                double totalMass = bodies.Sum(body => body.Mass);
                position = 1 / totalMass * position;
                return new BodyDomain(BarycentrumName, totalMass, position, velocity);
            }
            else if (bodies.Count == 1)
            {
                return new BodyDomain(BarycentrumName, bodies[0].Mass, bodies[0].Position, bodies[0].Velocity);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
