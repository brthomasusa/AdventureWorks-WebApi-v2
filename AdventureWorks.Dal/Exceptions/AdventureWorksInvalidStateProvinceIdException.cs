using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksInvalidStateProvinceIdException : AdventureWorksException
    {
        public AdventureWorksInvalidStateProvinceIdException() { }

        public AdventureWorksInvalidStateProvinceIdException(string message) : base(message) { }

        public AdventureWorksInvalidStateProvinceIdException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}