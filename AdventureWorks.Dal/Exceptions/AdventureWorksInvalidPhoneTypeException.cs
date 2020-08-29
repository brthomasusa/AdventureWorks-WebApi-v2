using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidPhoneTypeException : AdventureWorksException
    {
        public AdventureWorksInvalidPhoneTypeException() { }

        public AdventureWorksInvalidPhoneTypeException(string message)
         : base(message) { }

        public AdventureWorksInvalidPhoneTypeException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}