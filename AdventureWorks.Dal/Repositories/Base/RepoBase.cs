using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Dal.Repositories.Base
{
    public abstract class RepoBase<T> : IRepo<T> where T : EntityBase, new()
    {
        private readonly bool _disposeContext;

        public DbSet<T> Table { get; }

        public AdventureWorksContext Context { get; }

        public (string Schema, string TableName) TableSchemaAndName
        {
            get
            {
                var metaData = Context.Model
                    .FindEntityType(typeof(T).FullName)
                    .SqlServer();

                return (metaData.Schema, metaData.TableName);
            }
        }

        public bool HasChanges => Context.ChangeTracker.HasChanges();

        protected RepoBase(AdventureWorksContext ctx)
        {
            Context = ctx;
            Table = Context.Set<T>();
        }

        protected RepoBase(DbContextOptions<AdventureWorksContext> options)
            : this(new AdventureWorksContext(options))
        {
            _disposeContext = true;
        }

        public virtual void Dispose()
        {
            if (_disposeContext)
            {
                Context.Dispose();
            }
        }

        public virtual T Find(params object[] keyValues)
            => Table.Find(keyValues);

        public virtual T Find(Expression<Func<T, bool>> predicate)
            => Table.Where(predicate)
                .FirstOrDefault();

        public T FindAsNoTracking(Expression<Func<T, bool>> predicate)
            => Table.Where(predicate)
                .AsNoTracking()
                .FirstOrDefault();

        public T FindIgnoreQueryFilters(Expression<Func<T, bool>> predicate)
            => Table.IgnoreQueryFilters()
                .FirstOrDefault(predicate);

        public virtual IEnumerable<T> GetAll() => Table;

        public virtual IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy)
            => Table.OrderBy(orderBy);

        public IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new AdventureWorksConcurrencyExeception("A concurrency error occurred", ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new AdventureWorksRetryLimitExceededException("There is a problem with your connection.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                throw new AdventureWorksException("An error occurred while updating the database", ex);
            }
        }

        public void ExecuteInATransaction(Action actionToExecute)
        {
            var strategy = Context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using (var trans = Context.Database.BeginTransaction())
                {
                    actionToExecute();
                    trans.Commit();
                }
            });
        }
    }
}