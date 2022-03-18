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
