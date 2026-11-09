namespace Sas.Orbit.Service.Exceptions
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
    }
}
