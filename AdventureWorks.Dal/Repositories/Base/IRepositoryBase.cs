using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AdventureWorks.Dal.Repositories.Base
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void ExecuteInATransaction(Action actionToExecute);

        Task Save();
    }
}