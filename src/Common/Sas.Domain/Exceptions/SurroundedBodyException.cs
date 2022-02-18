using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Domain.Exceptions
{
    public class SurroundedBodyException : Exception
    {
        public SurroundedBodyException()
        {
        }

        public SurroundedBodyException(string message)
            : base(message)
        {
        }

        public SurroundedBodyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
