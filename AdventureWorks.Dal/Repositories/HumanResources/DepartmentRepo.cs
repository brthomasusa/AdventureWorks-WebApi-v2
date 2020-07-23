using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;

namespace AdventureWorks.Dal.Repositories.HumanResources
{
    public class DepartmentRepo : RepoBase<Department>, IDepartmentRepo
    {
        public DepartmentRepo(AdventureWorksContext context) : base(context) { }

        public DepartmentRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }

        public override int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var msg = "Error: The department you are trying to delete does not exist. Try refreshing your screen.";

                throw new AdventureWorksConcurrencyExeception(msg, ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new AdventureWorksRetryLimitExceededException("There is a problem with your connection.", ex);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Message.Contains("AK_Department_Name", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate department name!", ex);
                    }
                }

                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
        }
    }
}