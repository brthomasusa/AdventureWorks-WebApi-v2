using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Linq.Expressions;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Base
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        protected AdventureWorksContext DbContext { get; }
        protected ILoggerManager RepoLogger { get; }

        public RepositoryBase(AdventureWorksContext context, ILoggerManager logger)
        {
            DbContext = context;
            RepoLogger = logger;
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

        public async void Save()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var msg = "Error: The record that you are trying to update or delete does not exist. Try refreshing your screen.";
                RepoLogger.LogError($"Record does not exist: {ex.Message}");
                throw new AdventureWorksConcurrencyExeception(msg, ex);
            }
            catch (RetryLimitExceededException ex)
            {
                RepoLogger.LogError(ex.Message);
                throw new AdventureWorksRetryLimitExceededException("There is a problem with your connection.", ex);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Message.Contains("AK_Vendor_AccountNumber", StringComparison.OrdinalIgnoreCase))
                    {
                        RepoLogger.LogError($"Duplicate vendor account number: {ex.InnerException.Message}");
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate vendor account number!", ex);
                    }
                    else if (sqlException.Message.Contains("IX_Address_AddressLine1_AddressLine2_City_StateProvinceID_PostalCode", StringComparison.OrdinalIgnoreCase))
                    {
                        RepoLogger.LogError($"Duplicate entity address: {ex.InnerException.Message}");
                        throw new AdventureWorksUniqueIndexException("Error: There is an existing entity with this address!", ex);
                    }
                    else if (sqlException.Message.Contains("AK_Employee_LoginID", StringComparison.OrdinalIgnoreCase))
                    {
                        RepoLogger.LogError($"Duplicate employee login: {ex.InnerException.Message}");
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate employee login!", ex);
                    }
                    else if (sqlException.Message.Contains("AK_Employee_NationalIDNumber", StringComparison.OrdinalIgnoreCase))
                    {
                        RepoLogger.LogError($"Duplicate national ID number: {ex.InnerException.Message}");
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate employee national ID number!", ex);
                    }
                    else if (sqlException.Message.Contains("AK_Department_Name", StringComparison.OrdinalIgnoreCase))
                    {
                        RepoLogger.LogError($"Duplicate HR department: {ex.InnerException.Message}");
                        throw new AdventureWorksUniqueIndexException("Error: This operation would result in a duplicate HR department!", ex);
                    }
                }

                RepoLogger.LogError(ex.Message);
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                RepoLogger.LogError(ex.Message);
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