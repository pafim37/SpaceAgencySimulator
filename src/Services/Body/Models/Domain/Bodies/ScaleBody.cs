using Sas.Body.Service.Models.Domain.Orbits;
using Sas.Mathematica.Service;

namespace Sas.Body.Service.Models.Domain.Bodies
{
    public static class Scale
    {
        private const double ScaleFactor = 100;
        public static BodyDomain ScaleBody(BodyDomain body)
        {
            double scale = ScaleFactor / Constants.EarthPeriapsis;
            body.Position *= scale;
            body.SphereOfInfluenceRadius *= scale;
            return body;
        }

        public static PositionedOrbit ScaleOrbit(PositionedOrbit orbit)
        {
            double scale = ScaleFactor / Constants.EarthPeriapsis;

            for (int i = 0; i < orbit.Points.Count; i++)
            {
                orbit.Points[i].X *= scale;
                orbit.Points[i].Y *= scale;
                orbit.Points[i].Z *= scale;
            }

            return orbit;
        }
    }
}