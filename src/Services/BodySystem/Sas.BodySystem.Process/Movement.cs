using Sas.BodySystem.Process.Models.Domain.Bodies;
using Sas.BodySystem.Process.Models.Domain.Orbits;
using Sas.Mathematica.Service.Matrices;
using Sas.Mathematica.Service.Rotation;
using Sas.Mathematica.Service.Vectors;
using Sas.Orbit.Service.Orbits.OrbitalElements;
using Sas.Orbit.Service.Orbits.Primitives;

namespace Sas.BodySystem.Process
{
    public static class Movement
    {
        public static void Move(List<IOrbit> orbits, List<BodyDomain> bodies, double t)
        {
            foreach (IOrbit orbit in orbits)
            {
                BodyDomain? body = bodies.FirstOrDefault(b => b.Name == orbit.Name);
                if (body is null)
                {
                    continue;
                }
                BodyDomain? other = bodies.FirstOrDefault(b => b.Name == body.ParentName);
                Vector dr = Move(orbit.OrbitElements, t);
                body.Position = dr + other!.Position;
                orbit.UpdateCenterOfPoints(other.Position);
            }
        }

        private static Vector Move(IOrbitElements orbDesc, double t)
        {
            double meanAnomalyAtTime = orbDesc.MeanAnomaly + orbDesc.MeanMotion * t;
            double e = orbDesc.Eccentricity;
            double a = orbDesc.SemiMajorAxis ?? 0;
            Vector vector;
            if (orbDesc.OrbitType == OrbitType.Hyperbolic)
            {
                double anomaly = SolveHyperbolicKeplerEquation(meanAnomalyAtTime, e);
                double axisLength = Math.Abs(a);
                double halfSinh = Math.Sinh(anomaly / 2);
                vector = new(
                    axisLength * ((e - 1) - 2 * halfSinh * halfSinh),
                    axisLength * Math.Sqrt((e - 1) * (e + 1)) * Math.Sinh(anomaly),
                    0);
            }
            else
            {
                double eccentricAnomaly = SolveKeplerEquation(meanAnomalyAtTime, e);
                double trueAnomaly = EccentricToTrueAnomaly(eccentricAnomaly, e);
                double radius = OrbitalRadius(a, e, trueAnomaly);
                vector = new(radius * Math.Cos(trueAnomaly), radius * Math.Sin(trueAnomaly), 0);
            }

            Matrix result = Rotation.RotationMatrix3D(orbDesc.ArgumentOfPeriapsis, orbDesc.Inclination, double.IsNaN(orbDesc.AscendingNode) ? 0 : orbDesc.AscendingNode);
            return (result * vector.AsMatrix()).ToVector();
        }

        private static double SolveHyperbolicKeplerEquation(double meanAnomaly, double eccentricity)
        {
            if (meanAnomaly == 0 || !double.IsFinite(meanAnomaly))
            {
                return meanAnomaly;
            }

            double magnitude = Math.Abs(meanAnomaly);
            // The positive root of e sinh(H) - H = |M| lies in this bracket for e > 1.
            double lower = Math.Asinh(magnitude / eccentricity);
            double upper = Math.Min(lower + 2, magnitude / (eccentricity - 1));
            for (int iteration = 0; iteration < 64; iteration++)
            {
                double midpoint = lower + (upper - lower) / 2;
                if (midpoint == lower || midpoint == upper)
                {
                    break;
                }

                if (eccentricity * Math.Sinh(midpoint) - midpoint < magnitude)
                {
                    lower = midpoint;
                }
                else
                {
                    upper = midpoint;
                }
            }

            return Math.CopySign(lower + (upper - lower) / 2, meanAnomaly);
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
            {
                m -= 2.0 * Math.PI;
            }
            else if (m < -Math.PI)
            {
                m += 2.0 * Math.PI;
            }

            double E = (e < 0.8) ? m : Math.PI;

            for (int i = 0; i < KEPLER_MAX_IT; i++)
            {
                double f = E - e * Math.Sin(E) - m;
                double fPrime = 1.0 - e * Math.Cos(E);
                double dE = -f / fPrime;
                E += dE;

                if (Math.Abs(dE) < KEPLER_TOL)
                {
                    break;
                }
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
            {
                th += 2.0 * Math.PI;
            }

            return th;
        }
    }
}
