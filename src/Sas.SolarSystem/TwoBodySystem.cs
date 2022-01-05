using Sas.BodySystem.Models;
using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.BodySystem
{
    public class TwoBodySystem
    {
        private readonly IList<Body> _bodies;


        public TwoBodySystem(IList<Body> bodies)
        {
            _bodies = bodies;
        }

        public TwoBodySystem(Body body1, Body body2)
        {
            _bodies = new List<Body>
            {
                body1,
                body2
            };
        }

        /// <summary>
        /// Return barycenter position
        /// </summary>
        /// <returns></returns>
        public Vector GetBarycenter() => FindCenterOfMass();

        /// <summary>
        /// G * (M + m)
        /// </summary>
        public double U
        {
            get
            {
                double totalMass = 0;
                foreach (var body in _bodies)
                {
                    totalMass += body.Mass;
                }
                return totalMass * Constants.G;
            }
        }

        public void CalibrateBarycenterForZero()
        {
            var barycenterPosition = GetBarycenter();
            foreach (var body in _bodies)
            {
                body.AbsolutePosition -= barycenterPosition;
            }
        }

        private Vector FindCenterOfMass()
        {
            double sumMass = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            foreach (Body body in _bodies)
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
