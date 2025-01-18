namespace Sas.Body.Service.Models.Domain.Orbits.Helpers
{
    public static class CalculateOrbitFactory
    {

        public static Orbit? Calculate(BodyDomain body, BodyDomain other, double G)
        {
            try
            {
                return TwoBodyProblemOrbit.Calculate(body, other, G);
            }
            catch
            {
                // TODO: create another orbit
                // TOD0:log it
                return null;
            }
        }
        
    }
}
