using Sas.Mathematica.Service.Vectors;

namespace Sas.Common.Extesions
{
    public static class ArrayExtension
    {
        public static Vector ToVector(this double[] array)
        {
            return new Vector(array);
        }
    }
}
