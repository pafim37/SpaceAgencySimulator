﻿using Sas.Mathematica.Service.Vectors;

namespace Sas.Mathematica.Service.Extensions
{
    public static class ArrayExtension
    {
        public static Vector ToVector(this double[] array)
        {
            return new Vector(array);
        }
    }
}
