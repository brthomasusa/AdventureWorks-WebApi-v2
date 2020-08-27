using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidEntityIdException : AdventureWorksException
    {
        public AdventureWorksInvalidEntityIdException() { }

        public AdventureWorksInvalidEntityIdException(string message) : base(message) { }

        public AdventureWorksInvalidEntityIdException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}
