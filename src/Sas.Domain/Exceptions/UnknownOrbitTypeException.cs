using System.Runtime.Serialization;

namespace Sas.Domain.Exceptions
{
    public class UnknownOrbitTypeException : Exception
    {
        public UnknownOrbitTypeException()
        {
        }

        public UnknownOrbitTypeException(string? message) : base(message)
        {
        }

        public UnknownOrbitTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnknownOrbitTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
