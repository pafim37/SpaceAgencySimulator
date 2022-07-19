using Sas.Mathematica.Service.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Common
{
    public static class ArrayExtension
    {
        public static Vector ToVector(this double[] array)
        {
            return new Vector(array);
        }
    }
}
