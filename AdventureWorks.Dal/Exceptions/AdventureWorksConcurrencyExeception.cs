using System;

namespace AdventureWorks.Dal.Exceptions
{
    public class AdventureWorksConcurrencyExeception : Exception
    {
        public AdventureWorksConcurrencyExeception() { }

        public AdventureWorksConcurrencyExeception(string message) : base(message) { }

        public AdventureWorksConcurrencyExeception(string message, Exception innerException)
            : base(message, innerException) { }

    }
}