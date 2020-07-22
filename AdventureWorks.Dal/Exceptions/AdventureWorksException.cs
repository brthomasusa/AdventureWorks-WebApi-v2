using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksException : Exception
    {
        public AdventureWorksException() { }

        public AdventureWorksException(string message) : base(message) { }

        public AdventureWorksException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}