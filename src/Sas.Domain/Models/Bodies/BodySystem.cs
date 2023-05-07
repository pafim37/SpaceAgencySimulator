using Sas.Domain.Bodies.BodyExtensions;
using Sas.Domain.Models.Orbits;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Domain.Models.Bodies
{
    public class BodySystem
    {
        #region fields

        private readonly List<Body> _bodies;
        private readonly List<OrbitHolder> _orbitsDescription;

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
        public BodySystem(IEnumerable<Body> bodies)
        {
            _bodies = bodies.ToList();
            _orbitsDescription = new();
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
            var sortBodies = _bodies.OrderBy(x => x.Mass).ToList();
            for (int i = 0; i < sortBodies.Count - 1; i++)
            {
                var currBody = sortBodies[i];
                Body? resultBody = null;
                double distance = double.MaxValue;
                for (int j = i + 1; j < sortBodies.Count; j++)
                {
                    var nextBody = sortBodies[j];
                    var relativeDistance = currBody.GetPositionRelatedTo(nextBody).Magnitude;
                    var influence = nextBody.GetSphereOfInfluenceRelatedTo(currBody);
                    if (relativeDistance < distance && influence > relativeDistance)
                    {
                        distance = relativeDistance;
                        resultBody = nextBody;
                    }
                }
                _orbitsDescription.Add(
                    new OrbitHolder()
                    {
                        Name = currBody.Name,
                        SurroundedBodyName = resultBody?.Name,
                        Orbit = OrbitFactory.CalculateOrbit(
                            currBody.GetPositionRelatedTo(resultBody),
                            currBody.GetVelocityRelatedTo(resultBody),
                            Constants.G * (currBody.Mass + resultBody.Mass))
                    }); ;
            }
            _orbitsDescription.Add(
                new OrbitHolder()
                {
                    Name = sortBodies[sortBodies.Count - 1].Name,
                    SurroundedBodyName = Barycentrum.Name,
                    Orbit = OrbitFactory.CalculateOrbit(
                            sortBodies[sortBodies.Count - 1].GetPositionRelatedTo(Barycentrum),
                            sortBodies[sortBodies.Count - 1].GetVelocityRelatedTo(Barycentrum),
                            Constants.G * (sortBodies[sortBodies.Count - 1].Mass + Barycentrum.Mass))
                });
        }
        public void CalibrateBarycenterToZero()
        {
            Body barycenter = GetBarycenter();
            foreach (Body body in _bodies)
            {
                body.Position -= barycenter.Position;
            }
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
            return new Body("Barycentrum", totalMass, position, Vector.Zero);
        }
        private double GetU()
        {
            double totalMass = _bodies.Sum(body => body.Mass);
            return totalMass * Constants.G;
        }
        #endregion
    }
}
