using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidAddressTypeException : AdventureWorksException
    {
        public AdventureWorksInvalidAddressTypeException() { }

        public AdventureWorksInvalidAddressTypeException(string message) : base(message) { }

        public AdventureWorksInvalidAddressTypeException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}