using Sas.Mathematica;
using Sas.SolarSystem.Models;
using Sas.SolarSystem.Orbits;

namespace Sas.SolarSystem
{
    public class SolarSystem
    {
        private IList<Body> _solarSystem;
        public IList<Body> GetBodies() => _solarSystem;
        public Vector GetBarycentrum() => FindCenterOfMass();

        public SolarSystem()
        {
            _solarSystem = new List<Body>() { new Body("Barycentrum", 0, Vector.Zero, Vector.Zero) };
        }

        public void Init()
        {
            CreateBodies();
            FindAndAssignAttracted();
            AssignOribits();
        }

        public void AddBody(Body body)
        {
            _solarSystem.Add(body);
        }

        private void UpdateSolarSystem()
        {
            FindAndAssignAttracted();
            AssignOribits();
        }
        
        private void CreateBodies()
        {
            //Body Sun = new Body.Builder()
            //    .Name("Sun")
            //    .Mass(Constants.SolarMass)
            //    .AbsolutePosition(new Vector(0, 0, 0))
            //    .AbsoluteVelocity(new Vector(0, 0, 0))
            //    .Type(BodyType.Star)
            //    .Build();

            //Body Earth = new Body.Builder()
            //    .Name("Earth")
            //    .Mass(Constants.EarthMass)
            //    .AbsolutePosition(new Vector(Constants.EarthPeriapsis, 0, 0))
            //    .AbsoluteVelocity(new Vector(0, Constants.EarthMaxVelocity, 0))
            //    .Type(BodyType.Planet)
            //    .Build();

            //Body Moon = new Body.Builder()
            //    .Name("Moon")
            //    .Mass(Constants.MoonMass)
            //    .AbsolutePosition(new Vector(Constants.EarthPeriapsis - Constants.MoonPeriapsis, 0, 0))
            //    .AbsoluteVelocity(new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0))
            //    .Type(BodyType.Moon)
            //    .Build();

            //Body Probe = new Body.Builder()
            //    .Name("Probe")
            //    .Mass(100)
            //    .AbsolutePosition(new Vector(Constants.EarthPeriapsis - Constants.MoonPeriapsis - 400, 0, 0))
            //    .AbsoluteVelocity(new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0))
            //    .Type(BodyType.Spacecraft)
            //    .Build();

            //_solarSystem.Add(Sun);
            //_solarSystem.Add(Earth);
            //_solarSystem.Add(Moon);
            //_solarSystem.Add(Probe);
        }

        private void FindAndAssignAttracted()
        {
            foreach (var body in _solarSystem)
            {
                // find closest bodies
                var closeBodiesList = _solarSystem.OrderBy(x => body.GetPositionRelatedTo(x).Magnitude() ).ToList();
                for (int i = 1; i < closeBodiesList.Count; i++) // first is itself (distance = 0)
                {
                    Body closest = closeBodiesList[i];

                    if (closest.AbsolutePosition == null || body.AbsolutePosition == null ) throw new Exception();
                    CoordinateSystem cs = new CoordinateSystem(closest.AbsolutePosition);
                    cs.Cartesian(body.AbsolutePosition);

                    if (closest.GetSphereOfInfluence(body) >= body.GetPositionRelatedTo(closest).Magnitude())
                    {
                        body.SurroundedBody = closest;
                        break;
                    }
                    else
                    {
                        body.SurroundedBody = closeBodiesList[0];
                    }
                }
            }
        }

        private void AssignOribits()
        {
            foreach (var body in _solarSystem)
            {
                body.Orbit = Orbit.CreateOrbit(body.GetPositionRelatedToSurroundedBody(), body.GetVelocityRelatedToSurroundedBody(), body.U);
            }
        }

        private Vector FindCenterOfMass()
        {
            double sumMass = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            foreach (Body body in _solarSystem)
            {
                x += body.Mass * body.AbsolutePosition.X; 
                y += body.Mass * body.AbsolutePosition.Y; 
                z += body.Mass * body.AbsolutePosition.Z;
                sumMass += body.Mass;
            }
            return 1 / sumMass * new Vector(x, y, z);
        }

        
    }
}
