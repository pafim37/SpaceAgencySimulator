using Sas.Mathematica;
using Sas.SolarSystem.Models;
using Sas.SolarSystem.Orbits;

namespace Sas.SolarSystem
{
    public class SolarSystem
    {
        private IList<Body> _solarSystem;

        public SolarSystem()
        {
            _solarSystem = new List<Body>();
        }

        public IList<Body> GetBodies() => _solarSystem;
        
        public void CreateDefaultSolarSystem()
        {
            Body Sun = new Body.Builder()
                .Name("Sun")
                .Mass(Constants.SolarMass)
                .AbsolutePosition(new Vector(0, 0, 0))
                .AbsoluteVelocity(new Vector(0, 0, 0))
                .Type(BodyType.Star)
                .Build();

            Body Earth = new Body.Builder()
                .Name("Earth")
                .Mass(Constants.EarthMass)
                .AbsolutePosition(new Vector(Constants.EarthPeriapsis, 0, 0))
                .AbsoluteVelocity(new Vector(0, Constants.EarthMaxVelocity, 0))
                .Type(BodyType.Planet)
                .Build();

            Body Moon = new Body.Builder()
                .Name("Moon")
                .Mass(Constants.MoonMass)
                .AbsolutePosition(new Vector(Constants.EarthPeriapsis - Constants.MoonPeriapsis, 0, 0))
                .AbsoluteVelocity(new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0))
                .Type(BodyType.Moon)
                .Build();

            Body Probe = new Body.Builder()
                .Name("Probe")
                .Mass(100)
                .AbsolutePosition(new Vector(Constants.EarthPeriapsis - Constants.MoonPeriapsis - 400, 0, 0))
                .AbsoluteVelocity(new Vector(0, Constants.MoonMaxVelocity + Constants.EarthMaxVelocity, 0))
                .Type(BodyType.Spacecraft)
                .Build();

            _solarSystem.Add(Sun);
            _solarSystem.Add(Earth);
            _solarSystem.Add(Moon);
            _solarSystem.Add(Probe);
        }
        public void FindAndAssignAttracted()
        {
            foreach (var body in _solarSystem)
            {
                // find closest body
                var closestList = _solarSystem.OrderBy(x => (body.AbsolutePosition - x.AbsolutePosition).Magnitude()).ToList();
                for (int i = 1; i < closestList.Count; i++) // last is itself (distance = 0)
                {
                    var closest = closestList[i];
                    CoordinateSystem cs = new CoordinateSystem(closest.AbsolutePosition);
                    cs.Cartesian(body.AbsolutePosition);
                    if (closest.GetSphereOfInfluence(body) >= (body.AbsolutePosition - closest.AbsolutePosition).Magnitude())
                    {
                        body.Attracted = closest;
                        break;
                    }
                    else
                    {
                        body.Attracted = closestList[0];
                    }
                }
            }
        }

        public void AssignOribit()
        {
            foreach (var body in _solarSystem)
            {
                Vector r = body.AbsolutePosition - body.Attracted.AbsolutePosition;
                Vector v = body.AbsoluteVelocity - body.Attracted.AbsoluteVelocity;
                double u = body.U;
                body.Orbit = Orbit.CreateOrbit(r, v, u);
            }
        }

        private void ValidateSolarSystem()
        {
            foreach (var body in _solarSystem)
            {
                if (body.Type == null)
                {
                    throw new Exception($"Body: {body.Name} has no type");
                }
            }
            // only one star
            if (_solarSystem.Where(x => x.Type == BodyType.Star).Count() != 1) throw new Exception("More than one star in Solar System");

            // star isn't attracted by other bodies
            if (_solarSystem.Where(x => x.Type == BodyType.Star).FirstOrDefault().Attracted.Type != BodyType.Star) throw new Exception("Star has no valid attracted body");

        }

    }
}
