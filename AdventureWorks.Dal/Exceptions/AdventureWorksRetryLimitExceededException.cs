using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksRetryLimitExceededException : AdventureWorksException
    {
        public AdventureWorksRetryLimitExceededException() { }

        public AdventureWorksRetryLimitExceededException(string message) : base(message) { }

        public AdventureWorksRetryLimitExceededException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}