using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidContactTypeException : AdventureWorksException
    {
        public AdventureWorksInvalidContactTypeException() { }

        public AdventureWorksInvalidContactTypeException(string message) : base(message) { }

        public AdventureWorksInvalidContactTypeException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}