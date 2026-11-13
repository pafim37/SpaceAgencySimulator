using Sas.BodySystem.Process.Extensions;
using Sas.BodySystem.Process.Models.Domain.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits;

namespace Sas.BodySystem.Process.BodySystems
{
    using Body = Sas.BodySystem.Process.Models.Domain.Bodies.Body;

    public class BodySystem(IEnumerable<Body> bodies, IPositionedOrbitFactory orbitFactory,
        double gravitationalConstant = Constants.G)
    {
        #region fields
        private readonly List<Body> bodies = [.. bodies];
        private readonly List<OrbitData> orbits = [];
        private const string BarycentrumName = "Barycentrum";
        private Body? barycenter;
        #endregion

        #region properties
        /// <summary>
        /// Gets gravitational constant.
        /// </summary>
        public double GravitationalConstant => gravitationalConstant;

        /// <summary>
        /// Gets center of mass of the system (Barycentrum).
        /// </summary>
        public Body? Barycentrum => barycenter;

        /// <summary>
        /// Gets list of bodies in current body system.
        /// </summary>
        public List<Body> Bodies => bodies;

        /// <summary>
        /// Gets list of orbits
        /// </summary>
        public IReadOnlyList<OrbitData> Orbits => orbits;
        #endregion

        #region public methods

        public void FindOrbits(CancellationToken cancellationToken = default)
        {
            List<OrbitData> calculatedOrbits = [];
            foreach (Body body in bodies)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (body.ParentName is null)
                {
                    continue;
                }

                Body parent = bodies.Single(candidate => candidate.Name == body.ParentName);
                double gravitationalParameter = GravitationalConstant * (parent.Mass + body.Mass);
                IPositionedOrbit orbit = orbitFactory.Create(
                    body.Name,
                    body.GetPositionRelatedTo(parent),
                    body.GetVelocityRelatedTo(parent),
                    gravitationalSourcePosition: parent.Position,
                    gravitationalParameter);
                calculatedOrbits.Add(new OrbitData
                {
                    Name = orbit.Name,
                    Points = [.. orbit.Points.Select(point => new OrbitPointData(point.X, point.Y, point.Z))]
                });
            }

            orbits.Clear();
            orbits.AddRange(calculatedOrbits);
        }

        /// <summary>
        /// Finds the barycenter, establishes the body hierarchy, and centers the system.
        /// </summary>
        public void UpdateBodySystem(CancellationToken cancellationToken)
        {
            // TODO: DO THIS method Task not void
            barycenter = FindBarycenter();
            EstablishHierarchy();
            CalibrateBarycenterToZero();
            FindOrbits(cancellationToken);
        }

        /// <summary>
        /// Finds parents of the bodies.
        /// </summary>
        public void EstablishHierarchy()
        {
            List<Body> sortBodies = [.. bodies.OrderByDescending(x => x.Mass)]; // Sun, Earth, Moon, Satellite...
            if (sortBodies.Count == 0)
            {
                return;
            }
            Body mostMassiveBody = sortBodies[0];
            if (sortBodies.Count > 1)
            {
                Body planet = sortBodies[1];
                planet.ParentName = mostMassiveBody.Name;
                planet.SphereOfInfluenceRadius = planet.GetSphereOfInfluenceRelatedTo(mostMassiveBody);
            }
            for (int i = 2; i < sortBodies.Count; i++)
            {
                Body body = sortBodies[i];
                Body parentBody = mostMassiveBody;
                double tmpDistance = double.MaxValue;
                for (int j = 1; j < i; j++)
                {
                    Body other = sortBodies[j];
                    double relativeDistance = body.GetPositionRelatedTo(other).Magnitude;
                    Body parentOtherBody = sortBodies.Find(b => b.Name == other.ParentName) ?? sortBodies.First();
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
        /// Calibrate barycenter to zero. It changes position of bodies and orbits.
        /// </summary>
        public void CalibrateBarycenterToZero()
        {
            if (barycenter != null)
            {
                foreach (Body body in bodies)
                {
                    body.Position -= barycenter.Position;
                    body.Velocity -= barycenter.Velocity;
                }
                barycenter.Position = Vector.Zero;
                barycenter.Velocity = Vector.Zero;
            }
        }

        #endregion

        #region private methods
        private Body? FindBarycenter()
        {
            if (bodies.Count > 1)
            {
                double maxMass = bodies.Max(b => b.Mass);
                IEnumerable<Body> mostMassiveBodies = bodies.Where(b => b.Mass == maxMass);
                Vector position = Vector.Zero;
                Vector velocity = Vector.Zero;
                foreach (Body body in mostMassiveBodies)
                {
                    velocity += body.Velocity;
                }
                foreach (Body body in bodies)
                {
                    position += body.Mass * body.Position;
                }
                double totalMass = bodies.Sum(body => body.Mass);
                position = 1 / totalMass * position;
                return new Body(BarycentrumName, totalMass, position, velocity);
            }
            else if (bodies.Count == 1)
            {
                return new Body(BarycentrumName, bodies[0].Mass, bodies[0].Position, bodies[0].Velocity);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
