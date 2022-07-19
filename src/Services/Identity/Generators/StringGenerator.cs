using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Generators
{
    internal static class StringGenerator
    {
        private static readonly Random _random = new Random();

        public static string Generate(int n)
        {
            string output = string.Empty;
            for (int i = 0; i < n; i++)
            {
                int flag =_random.Next(0, 2);
                if (flag == 0)
                {
                    output += Letter(false).ToString();
                }
                else if (flag == 1)
                {
                    output += Letter(true).ToString();
                }
                else
                {
                    output +=_random.Next(0, 9).ToString();               
                }
            }
            return output;
        }

        private static char Letter(bool isUpperCase)
        {
            char offset = isUpperCase ? 'A' : 'a';
            int letterOffset = 26;
            return (char)_random.Next(offset, offset + letterOffset);
        }
    }
}
