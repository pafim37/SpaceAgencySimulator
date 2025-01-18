namespace Sas.Body.Service.Exceptions
{
    public class TwoBodyProblemAssumptionNotSatisfiedException : Exception
    {
        public TwoBodyProblemAssumptionNotSatisfiedException()
        {
        }

        public TwoBodyProblemAssumptionNotSatisfiedException(string? message) : base(message)
        {
        }

        public TwoBodyProblemAssumptionNotSatisfiedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
