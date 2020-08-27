using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksNullEntityObjectException : AdventureWorksException
    {
        public AdventureWorksNullEntityObjectException() { }

        public AdventureWorksNullEntityObjectException(string message) : base(message) { }

        public AdventureWorksNullEntityObjectException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}