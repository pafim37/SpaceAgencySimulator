using Sas.BodySystem.Process.Extensions;
using Sas.BodySystem.Process.Models.Domain.Bodies;
using Sas.BodySystem.Process.Models.Domain.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Process.BodySystems
{

    public class BodySystem(IEnumerable<BodyDomain> bodies, IPositionedOrbitFactory orbitFactory,
        double gravitationalConstant = Constants.G)
    {
        #region fields
        private readonly List<BodyDomain> bodies = [.. bodies];
        private readonly List<IPositionedOrbit> orbits = [];
        private const string BarycentrumName = "Barycentrum";
        private BodyDomain? barycenter;
        #endregion

        #region properties
        /// <summary>
        /// Gets gravitational constant.
        /// </summary>
        public double GravitationalConstant => gravitationalConstant;

        /// <summary>
        /// Gets center of mass of the system (Barycentrum).
        /// </summary>
        public BodyDomain? Barycentrum => barycenter;

        /// <summary>
        /// Gets list of bodies in current body system.
        /// </summary>
        public IReadOnlyList<BodyDomain> Bodies => bodies;

        /// <summary>
        /// Gets list of orbits
        /// </summary>
        public List<IPositionedOrbit> Orbits => orbits;
        #endregion

        #region public methods

        /// <summary>
        /// Finds the barycenter, establishes the body hierarchy, and centers the system.
        /// </summary>
        public void UpdateBodySystem(float scale = 1000, double? atTime = 0, CancellationToken cancellationToken = default)
        {
            barycenter = FindBarycenter();
            List<string> orbitsToUpdates = EstablishHierarchy(); // body name is also name of the orbit
            CalibrateBarycenterToZero();
            double scaleFactor = ScaleBodySystem(scale: scale);
            FindOrbits(orbitsToUpdates, cancellationToken);
            if (atTime is not null)
            {
                atTime *= Math.Pow(scaleFactor, 1.5);
                Move(atTime.Value);
            }
        }

        public void Move(double t)
        {
            Movement.Move(Orbits, bodies, t);
        }

        public void FindOrbits(List<string> orbitsToUpdate, CancellationToken cancellationToken = default)
        {
            // TODO: refactor this loop
            foreach (string orbitName in orbitsToUpdate)
            {
                orbits.Remove(Orbits.Single(o => o.Name == orbitName));
            }
            foreach (BodyDomain body in bodies)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (body.ParentName is null || !orbitsToUpdate.Contains(body.Name))
                {
                    continue;
                }

                BodyDomain parent = bodies.Single(candidate => candidate.Name == body.ParentName);
                double gravitationalParameter = GravitationalConstant * (parent.Mass + body.Mass);
                IPositionedOrbit orbit = orbitFactory.Create(
                    body.Name,
                    body.GetPositionRelatedTo(parent),
                    body.GetVelocityRelatedTo(parent),
                    gravitationalSourcePosition: parent.Position,
                    gravitationalParameter);
                Orbits.Add(orbit);
            }
        }

        /// <summary>
        /// Finds parents of the bodies.
        /// </summary>
        public List<string> EstablishHierarchy()
        {
            List<string> bodiesThatChangedParent = [];
            List<BodyDomain> sortBodies = [.. bodies.OrderByDescending(x => x.Mass)]; // Sun, Earth, Moon, Satellite...
            if (sortBodies.Count == 0)
            {
                return [];
            }
            BodyDomain mostMassiveBody = sortBodies[0];
            if (sortBodies.Count > 1)
            {
                BodyDomain planet = sortBodies[1];
                if (planet.ParentName != mostMassiveBody.Name)
                {
                    bodiesThatChangedParent.Add(planet.Name);
                }
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
                if (body.ParentName != parentBody.Name)
                {
                    bodiesThatChangedParent.Add(body.Name);
                }
                body.ParentName = parentBody.Name;
                body.SphereOfInfluenceRadius = body.GetSphereOfInfluenceRelatedTo(parentBody);
            }
            return bodiesThatChangedParent;
        }

        /// <summary>
        /// Calibrate barycenter to zero. It changes position of bodies and orbits.
        /// </summary>
        public void CalibrateBarycenterToZero()
        {
            if (barycenter != null)
            {
                bodies.ForEach(body =>
                {
                    body.Position -= barycenter.Position;
                    body.Velocity -= barycenter.Velocity;
                });
                barycenter.Position = Vector.Zero;
                barycenter.Velocity = Vector.Zero;
            }
        }

        public BodyDomain? FindBarycenter()
        {
            if (bodies.Count > 1)
            {
                Vector position = Vector.Zero;
                Vector velocity = Vector.Zero;
                double maxMass = double.MinValue;
                double totalMass = 0;
                foreach (BodyDomain body in bodies)
                {
                    position += body.Mass * body.Position;
                    totalMass += body.Mass;

                    if (body.Mass > maxMass)
                    {
                        maxMass = body.Mass;
                        velocity = body.Velocity;
                    }
                    else if (body.Mass == maxMass)
                    {
                        velocity += body.Velocity;
                    }
                }

                position = 1 / totalMass * position;
                return new BodyDomain()
                {
                    Name = BarycentrumName,
                    Mass = totalMass,
                    Position = position,
                    Velocity = velocity
                };
            }
            else if (bodies.Count == 1)
            {
                return new BodyDomain
                {
                    Name = BarycentrumName,
                    Mass = bodies[0].Mass,
                    Position = bodies[0].Position,
                    Velocity = bodies[0].Velocity
                };
            }
            else
            {
                return null;
            }
        }

        public double ScaleBodySystem(float scale = 1000)
        {
            if (barycenter is null || scale == 0)
            {
                return 0;
            }
            double maxDistance = bodies.Max(body => body.GetPositionRelatedTo(barycenter).Magnitude);
            if (maxDistance > 0)
            {
                double scaleFactor = scale / maxDistance;
                bodies.ForEach(body =>
                {
                    body.Position *= scaleFactor;
                    body.Velocity *= 1 / Math.Sqrt(scaleFactor);
                    body.SphereOfInfluenceRadius *= scaleFactor;
                });

                return scaleFactor;
            }
            return 0;
        }

        #endregion
    }
}
