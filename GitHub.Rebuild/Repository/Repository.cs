using GitHub.Rebuild.Data;
using GitHub.Rebuild.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Rebuild.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        
        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        { 
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            return query.SingleOrDefault();
        }


    }
}
