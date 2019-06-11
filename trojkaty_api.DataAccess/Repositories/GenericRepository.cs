using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace trojkaty_api.DataAccess.Repositories
{
    public class GenericRepository<C, T> : IGenericRepository<T> where T : class where C : DbContext
    {
        private readonly C _context;

        public GenericRepository(C context)
        {
            _context = context;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>();
            return query;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = _context.Set<T>().Where(expression);
            return query;
        }

        public virtual async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
