using Sas.BodySystem.Models;
using Sas.BodySystem.Orbits;
using Sas.Mathematica;

namespace Sas.BodySystem
{
    public class SolarSystem
    {
        private readonly IList<BodyOld> _solarSystem;
        public IList<BodyOld> GetBodies() => _solarSystem;
        public Vector GetBarycentrum() => FindCenterOfMass();

        public SolarSystem()
        {
            _solarSystem = new List<BodyOld>() { new BodyOld("Barycentrum", 0, Vector.Zero, Vector.Zero) };
        }

        public void Init()
        {
            //CreateBodies();
            CreatePoints();
            FindAndAssignAttracted();
            AssignOribits();
        }

        public void AddBody(BodyOld body)
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
            BodyOld Sun = new BodyOld("Sun", Constants.SolarMass, new Vector(0, 0, 0), new Vector(0, 0, 0));

            BodyOld Earth = new BodyOld("Earth", Constants.EarthMass, new Vector(Constants.EarthApoapsis, 0, 0), new Vector(0, Constants.EarthMinVelocity, 0));

            BodyOld Moon = new BodyOld("Moon", Constants.MoonMass, new Vector(0, Constants.EarthApoapsis - Constants.MoonPeriapsis, 0), new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0));

            BodyOld Probe = new BodyOld("Probe", 100, new Vector(Constants.EarthPeriapsis - Constants.MoonPeriapsis - 400, 0, 0), new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0));

            _solarSystem.Add(Sun);
            _solarSystem.Add(Earth);
            _solarSystem.Add(Moon);
            _solarSystem.Add(Probe);
        }

        private void CreatePoints()
        {
            BodyOld body1 = new("One", 1000000, new Vector(0, 50, 0), new Vector(0, 0, 0));
            BodyOld body2 = new("Two", 10000, new Vector(100, 50, 0), new Vector(0, 0, 0));
            BodyOld body3 = new("Three", 100, new Vector(100, 30, 0), new Vector(0, 0, 0));
            BodyOld body4 = new("Four", 1, new Vector(110, 30, 0), new Vector(0, 0, 0));

            _solarSystem.Add(body1);
            _solarSystem.Add(body2);
            _solarSystem.Add(body3);
            _solarSystem.Add(body4);
        }

        private void FindAndAssignAttracted()
        {
            foreach (var body in _solarSystem)
            {
                // find closest bodies
                var closeBodiesList = _solarSystem.OrderBy(x => body.GetPositionRelatedTo(x).Magnitude()).ToList();
                for (int i = 1; i < closeBodiesList.Count; i++) // first is itself (distance = 0)
                {
                    BodyOld closest = closeBodiesList[i];

                    if (closest.AbsolutePosition == null || body.AbsolutePosition == null) throw new Exception();
                    ReferenceSystem cs = new ReferenceSystem(closest.AbsolutePosition);
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

            foreach (BodyOld body in _solarSystem)
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
