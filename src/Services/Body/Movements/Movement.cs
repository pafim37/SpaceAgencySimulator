using Sas.Body.Service.Models.Domain.Bodies;
using Sas.Body.Service.Models.Domain.Orbits;
using Sas.Body.Service.Models.Domain.Orbits.OrbitDescriptions;
using Sas.Body.Service.Models.Domain.Orbits.Points;
using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Movements
{
    public static class Movement
    {
        public static void Move(List<PositionedOrbit> orbits, List<BodyDomain> bodies, double t)
        {
            foreach (PositionedOrbit orbit in orbits)
            {
                BodyDomain? body = bodies.FirstOrDefault(b => b.Name == orbit.Name);
                if (body is null)
                {
                    continue;
                }
                BodyDomain? other = bodies.FirstOrDefault(b => b.Name == body.ParentName);
                Vector dr = Move(orbit.OrbitDescription, t);
                body.Position = dr + other!.Position;
                List<Point> orbitPoints = orbit.UpdateCenterOfPoints(other);
            }
        }

        private static Vector Move(IOrbitDescription orbDesc, double t)
        {
            double meanAnomalyAtTime = orbDesc.MeanAnomaly + orbDesc.MeanMotion * t;
            double E = SolveKeplerEquation(meanAnomalyAtTime, orbDesc.Eccentricity);
            double th = EccentricToTrueAnomaly(E, orbDesc.Eccentricity);
            double r = OrbitalRadius(orbDesc.SemiMajorAxis ?? 0, orbDesc.Eccentricity, th);
            double x = r * Math.Cos(th);
            double y = r * Math.Sin(th);
            double z = 0;
            Vector vector = new(x, y, z);
            Matrix result = Rotation.RotationMatrix3D(orbDesc.ArgumentOfPeriapsis, orbDesc.Inclination, orbDesc.AscendingNode);
            return (result * vector.AsMatrix()).ToVector();
        }
        private static double OrbitalRadius(double a, double e, double th)
        {
            return a * (1 - e * e) / (1 + e * Math.Cos(th));
        }
        private static double SolveKeplerEquation(double m, double e)
        {
            const int KEPLER_MAX_IT = 50;
            const double KEPLER_TOL = 1e-12;

            m = m % (2.0 * Math.PI);
            if (m > Math.PI)
                m -= 2.0 * Math.PI;
            else if (m < -Math.PI)
                m += 2.0 * Math.PI;

            double E = (e < 0.8) ? m : Math.PI;

            for (int i = 0; i < KEPLER_MAX_IT; i++)
            {
                double f = E - e * Math.Sin(E) - m;
                double fPrime = 1.0 - e * Math.Cos(E);
                double dE = -f / fPrime;
                E += dE;

                if (Math.Abs(dE) < KEPLER_TOL)
                    break;
            }

            return E; // eccentric anomaly

        }

        private static double EccentricToTrueAnomaly(double E, double e)
        {
            double factor = Math.Sqrt((1.0 + e) / (1.0 - e));
            double tan_th_over_2 = factor * Math.Tan(E / 2.0);
            double th = 2.0 * Math.Atan(tan_th_over_2);

            // normalization to [0, 2π)
            th = th % (2.0 * Math.PI);
            if (th < 0)
                th += 2.0 * Math.PI;

            return th;
        }
    }
}
