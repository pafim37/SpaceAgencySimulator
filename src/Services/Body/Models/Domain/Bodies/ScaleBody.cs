using Sas.Mathematica.Service;

namespace Sas.Body.Service.Models.Domain.Bodies
{
    public static class ScaleBody
    {
        private const double ScaleFactor = 100;
        public static BodyDomain Scale(BodyDomain body)
        {
            double scale = ScaleFactor / Constants.EarthPeriapsis;
            body.Radius = 1 + Math.Log10(body.Mass);
            body.Position *= Constants.EarthPeriapsis * scale;
            body.Velocity *= Constants.EarthMaxVelocity * scale;
            body.Mass *= Constants.EarthMass * Math.Pow(scale, 3);
            return body;
        }
    }
}