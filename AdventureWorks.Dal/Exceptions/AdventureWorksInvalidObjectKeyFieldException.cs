using System;
namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidObjectKeyFieldException : AdventureWorksException
    {
        public AdventureWorksInvalidObjectKeyFieldException() { }

        public AdventureWorksInvalidObjectKeyFieldException(string message) : base(message) { }

        public AdventureWorksInvalidObjectKeyFieldException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}