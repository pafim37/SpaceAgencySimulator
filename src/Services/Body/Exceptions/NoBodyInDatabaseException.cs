namespace Sas.Body.Service.Exceptions
{
    public class NoBodyInDatabaseException : Exception
    {
        public NoBodyInDatabaseException()
        {
        }

        public NoBodyInDatabaseException(string? message) : base(message)
        {
        }

        public NoBodyInDatabaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
