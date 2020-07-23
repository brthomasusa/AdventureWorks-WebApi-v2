using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksUniqueIndexException : AdventureWorksException
    {
        public AdventureWorksUniqueIndexException() { }

        public AdventureWorksUniqueIndexException(string message) : base(message) { }

        public AdventureWorksUniqueIndexException(string message, Exception innerException) : base(message, innerException) { }
    }
}