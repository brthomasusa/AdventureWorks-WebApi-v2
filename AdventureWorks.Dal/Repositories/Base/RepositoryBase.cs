using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Linq.Expressions;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;

namespace AdventureWorks.Dal.Repositories.Base
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AdventureWorksContext DbContext { get; }

        public RepositoryBase(AdventureWorksContext context)
        {
            DbContext = context;
        }

        public IQueryable<T> FindAll()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return DbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var msg = "Error: The vendor you are trying to update does not exist. Try refreshing your screen.";

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
                    if (sqlException.Message.Contains("AK_Vendor_AccountNumber", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate vendor account number!", ex);
                    }
                    else if (sqlException.Message.Contains("IX_Address_AddressLine1_AddressLine2_City_StateProvinceID_PostalCode", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new AdventureWorksUniqueIndexException("Error: There is an existing entity with this address!", ex);
                    }
                }

                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
        }

        public void ExecuteInATransaction(Action actionToExecute)
        {
            var strategy = DbContext.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using (var trans = DbContext.Database.BeginTransaction())
                {
                    actionToExecute();
                    trans.Commit();
                }
            });
        }

    }
}