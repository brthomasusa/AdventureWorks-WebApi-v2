using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdventureWorks.Dal.EfCode
{
    public class AdventureWorksContextFactory : IDesignTimeDbContextFactory<AdventureWorksContext>
    {
        public AdventureWorksContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorksContext>();
            var connectionString = @"Server=tcp:mssql-svr,1433;Database=AdventureWorks_Testing;User Id=sa;Password=Info99Gum;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

            return new AdventureWorksContext(optionsBuilder.Options);
        }
    }
}