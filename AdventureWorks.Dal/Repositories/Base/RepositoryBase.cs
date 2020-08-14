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
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AdventureWorksContext ctx { get; set; }

        public RepositoryBase(AdventureWorksContext context)
        {
            ctx = context;
        }

        public IQueryable<T> FindAll()
        {
            return ctx.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return ctx.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            ctx.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            ctx.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            ctx.Set<T>().Remove(entity);
        }
    }
}