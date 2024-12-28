namespace Sas.Body.Service.Exceptions
{
    public class BodyAlreadyExistsException : Exception
    {
        public BodyAlreadyExistsException()
        {
        }

        public BodyAlreadyExistsException(string? message) : base(message)
        {
        }

        public BodyAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
