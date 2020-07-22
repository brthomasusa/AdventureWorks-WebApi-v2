using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidVendorException : AdventureWorksException
    {
        public AdventureWorksInvalidVendorException() { }

        public AdventureWorksInvalidVendorException(string message) : base(message) { }

        public AdventureWorksInvalidVendorException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}