using Sas.Mathematica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Sandbox.Models
{
    // TODO (pafim37): refactor this
    public class InfluenceArea
    {
      

        public void WriteCirculatedBody(Body body, IList<Body> bodies)
        {
            for (int i = 0; i < bodies.Count - 1; i++)
            {
                if(!body.Name.Equals(bodies[i]))
                {
                    Console.WriteLine("x");
                }
            }
        }

        
    }
}
