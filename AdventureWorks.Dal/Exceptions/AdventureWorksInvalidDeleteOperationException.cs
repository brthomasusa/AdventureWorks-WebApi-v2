using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidDeleteOperationException : Exception
    {
        public AdventureWorksInvalidDeleteOperationException() { }

        public AdventureWorksInvalidDeleteOperationException(string message) : base(message) { }

        public AdventureWorksInvalidDeleteOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}