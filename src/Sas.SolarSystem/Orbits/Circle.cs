using Sas.Mathematica;

namespace Sas.BodySystem.Orbits
{
    internal class Circle : Orbit
    {
        public Circle(Vector positionRelated, Vector velocityRelated, double u) : base(positionRelated, velocityRelated, u)
        {
        }
    }
}
