using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Dal.Repositories.Base
{
    public interface IRepo<T> : IDisposable where T : EntityBase
    {
        DbSet<T> Table { get; }

        AdventureWorksContext Context { get; }

        (string Schema, string TableName) TableSchemaAndName { get; }

        bool HasChanges { get; }

        T Find(params object[] keyValues);

        T Find(Expression<Func<T, bool>> predicate);

        T FindAsNoTracking(Expression<Func<T, bool>> predicate);

        T FindIgnoreQueryFilters(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy);

        IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take);

        int Add(T entity, bool persist = true);

        int AddRange(IEnumerable<T> entities, bool persist = true);

        int Update(T entity, bool persist = true);

        int UpdateRange(IEnumerable<T> entities, bool persist = true);

        int Delete(T entity, bool persist = true);

        int DeleteRange(IEnumerable<T> entities, bool persist = true);

        int SaveChanges();
    }
}