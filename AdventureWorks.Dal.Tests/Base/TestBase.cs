using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AdventureWorks.Dal.EfCode;

namespace AdventureWorks.Dal.Tests.Base
{
    public class TestBase : IDisposable
    {
        protected AdventureWorksContext ctx;

        public TestBase()
        {
            ResetContext();
        }

        public virtual void Dispose()
        {
            ctx.Dispose();
        }

        protected void ResetContext()
        {
            ctx = new AdventureWorksContextFactory().CreateDbContext(null);
        }

        protected void ExecuteInATransaction(Action actionToExecute)
        {
            var strategy = ctx.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using (var trans = ctx.Database.BeginTransaction())
                {
                    actionToExecute();
                    // trans.Commit();
                    trans.Rollback();
                }
            });
        }

        protected (AdventureWorksContext context1, AdventureWorksContext context2) GetTwoContextsWithSharedConnection()
        {
            var optionBuilder = new DbContextOptionsBuilder<AdventureWorksContext>();
            var connectionString = @"Server=tcp:mssql-svr,1433;Database=AdventureWorks_Testing;User Id=sa;Password=Info99Gum;MultipleActiveResultSets=true";

            optionBuilder.UseSqlServer(new SqlConnection(connectionString), options => options.EnableRetryOnFailure());
            optionBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            var opt = optionBuilder.Options;

            return (new AdventureWorksContext(opt), new AdventureWorksContext(opt));
        }
    }
}