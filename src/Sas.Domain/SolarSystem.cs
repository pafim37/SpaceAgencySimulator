using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service;
using Sas.Mathematica.Service.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain
{
    public class SolarSystem
    {
        private readonly List<Body> _bodies;

        /// <summary>
        /// Center of mass (Barycentrum)
        /// </summary>
        public Body Barycentrum => GetBarycenter();

        /// <summary>
        /// G * (M + m1 + m2 + ...)
        /// </summary>
        public double U { get; }

        /// <summary>
        /// Update state of the Solar System
        /// </summary>
        public void Update()
        {
            CalibrateBarycenterForZero();
            //FindAndAssignSurroundedBody();
            //CalculateOribit();
        }

        /// <summary>
        /// Returns list of the body in solar system
        /// </summary>
        /// <returns></returns>
        public List<Body> GetBodies() => _bodies;

        public SolarSystem(List<Body> bodies)
        {
            // TODO: Is it necessary? What if I would like to have one planet, then is orbit null or point?
            if (bodies.Count < 2)
                throw new ArgumentException("Solar system has not enough bodies");

            _bodies = bodies;
            U = GetU();
            Update();

        }

        #region private methods

        //private void CalculateOribit()
        //{
        //    foreach (Body b in _bodies)
        //    {
        //        b.UpdateOrbit();
        //    }
        //}

        //private void FindAndAssignSurroundedBody()
        //{
        //    var sortBodies = _bodies.OrderBy(x => x.Mass).ToList();
        //    // first is itself (distance = 0)
        //    for (int i = 0; i < sortBodies.Count; i++)
        //    {
        //        var body = sortBodies[i];
        //        Body closest = null;
        //        double distance = double.MaxValue;
        //        for (int j = i+1; j < sortBodies.Count; j++)
        //        {
        //            var nextBody = sortBodies[j];
        //            var d = body.GetPositionRelatedTo(nextBody).Magnitude;
        //            if (d < distance && nextBody.GetSphereOfInfluence(body) >= d)
        //            {
        //                distance = d;
        //                body.SurroundedBody = nextBody;
        //            }
        //        }
        //    }
        //    sortBodies[sortBodies.Count - 1].SurroundedBody = GetBarycenter();
        //}

        private void CalibrateBarycenterForZero()
        {
            var barycenter = GetBarycenter();
            foreach (var body in _bodies)
            {
                body.AbsolutePosition -= barycenter.AbsolutePosition;
            }
        }

        private Body GetBarycenter()
        {
            double totalMass = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var body in _bodies)
            {
                x += body.Mass * body.AbsolutePosition.X;
                y += body.Mass * body.AbsolutePosition.Y;
                z += body.Mass * body.AbsolutePosition.Z;
                totalMass += body.Mass;
            }
            Vector position =  1 / totalMass * new Vector(x, y, z);
            return new Body("Barycentrum", totalMass, position, Vector.Zero);
        }

        private double GetU()
        {
            double totalMass = 0;
            foreach (var body in _bodies)
            {
                totalMass += body.Mass;
            }
            return totalMass * Constants.G;
        }
        #endregion
    }
}
