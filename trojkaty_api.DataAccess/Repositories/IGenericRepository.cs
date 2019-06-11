using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace trojkaty_api.DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        IQueryable<T> FindBy(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        Task SaveAsync();
    }
}