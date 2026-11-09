using MediatR;
using Sas.BodySystem.Service.Extensions;
using Sas.BodySystem.Service.Models.Domain.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Models.Requests;
using Sas.Orbit.Service.Orbits;

namespace Sas.BodySystem.Service.BodySystems
{
    using Body = Sas.BodySystem.Service.Models.Domain.Bodies.Body;

    public class BodySystem(IMediator mediator, IEnumerable<Body> bodies, double gravitationalConst = Constants.G)
    {
        #region fields
        private readonly List<Body> bodies = [.. bodies];

        public List<OrbitData> Orbits { get; } = [];
        private const string BarycentrumName = "Barycentrum";
        private Body? barycenter;
        #endregion

        #region properties
        /// <summary>
        /// Gets gravitational constant.
        /// </summary>
        public double G => gravitationalConst;

        /// <summary>
        /// Gets center of mass of the system (Barycentrum).
        /// </summary>
        public Body? Barycentrum => barycenter;

        /// <summary>
        /// Gets list of bodies in current body system.
        /// </summary>
        public List<Body> Bodies => bodies;

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
        /// Finds orbits of the bodies.
        /// </summary>
        public async Task FindOrbits()
        {
            foreach (Body body in bodies)
            {
                if (body.ParentName is null)
                {
                    continue;
                }
                Body? ParentBody = bodies.FirstOrDefault(b => b.Name == body.ParentName);
                if (ParentBody is null)
                {
                    continue;
                }

                try
                {
                    double gravitationalParameter = G * (ParentBody.Mass + body.Mass);
                    PositionedOrbit orbit = await mediator.Send(new GetPositionedOrbitRequest(body.Name, body.Position, body.Velocity, ParentBody.Position, gravitationalParameter));

                    // TODO: Consider it
                    Orbits.Add(new OrbitData
                    {
                        Name = body.Name,
                        Points = [.. orbit.Points.Select(p => new OrbitPointData(p.X, p.Y, p.Z))],
                    });
                }
                catch
                {
                    // TODO: Don't leave it empty. Consider logging or handling the exception appropriately.
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
