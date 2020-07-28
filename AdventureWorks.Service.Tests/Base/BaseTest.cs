using System;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Initialization;

namespace AdventureWorks.Service.Tests.Base
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly string serviceAddress = "http://localhost:5000/";
        protected string rootAddress = string.Empty;

        public virtual void Dispose() { }

        protected void ResetDatabase()
        {
            SampleDataInitialization.InitializeData(new AdventureWorksContextFactory().CreateDbContext(null));
        }
    }
}