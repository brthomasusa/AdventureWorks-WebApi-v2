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
        public IQueryable<T> FindAll()
        {
            return null;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return null;
        }

        public void Create(T entity)
        {

        }

        public void Update(T entity)
        {

        }

        public void Delete(T entity)
        {

        }
    }
}