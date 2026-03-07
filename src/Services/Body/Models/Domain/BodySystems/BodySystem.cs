using Sas.Body.Service.Extensions.BodyExtensions;
using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits;
using Sas.Body.Service.Movements;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Models.Domain.BodySystems
{
    public class BodySystem(IEnumerable<BodyDomain> bodies, double gravitationalConst = Constants.G)
    {
        #region fields
        private readonly List<BodyDomain> bodies = bodies.ToList();
        private readonly List<PositionedOrbit> orbits = [];
        private const string BarycentrumName = "Barycentrum";
        private BodyDomain? barycenter;
        #endregion

        #region properties
        /// <summary>
        /// Gets gravitational constant.
        /// </summary>
        public double G => gravitationalConst;

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

        #region public methods

        /// <summary>
        /// It finds barycenter, establishes hierarchy between bodies and finds orbits. 
        /// </summary>
        public void UpdateBodySystem()
        {
            barycenter = FindBarycenter();
            EstablishHierarchy();
            CalibrateBarycenterToZero();
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
        public void FindOrbits(bool skipGetPoints = false)
        {
            foreach (BodyDomain body in bodies)
            {
                if (body.ParentName is null)
                {
                    continue;
                }
                BodyDomain? centerBody = bodies.FirstOrDefault(b => b.Name == body.ParentName);
                if (centerBody is null) continue;
                try
                {
                    PositionedOrbit orbit = OrbitFactory.GetOrbit(body, centerBody, G);
                    if (!skipGetPoints)
                    {
                        orbit.UpdateCenterOfPoints(centerBody);
                    }
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
                barycenter.Position = Vector.Zero;
                barycenter.Velocity = Vector.Zero;
            }
        }

        /// <summary>
        /// Advances the simulation by moving all bodies along their orbits for the specified time interval.
        /// </summary>
        /// <param name="t">The time interval, in seconds, by which to advance the simulation. Must be a non-negative value.</param>
        public void Move(double t)
        {
            Movement.Move(orbits, bodies, t);
        }
        #endregion

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
