using DomainEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SchoolMsContext _context;
        private DbSet<TEntity> dbset;

        public Repository()
        {
            this._context = new SchoolMsContext();
            this.dbset = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            if (entity == null) { 
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                dbset.Add(entity);
                _context.SaveChanges();


            }
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                dbset.Remove(entity);
                _context.SaveChanges();
            }
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbset;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(expression);
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbset.AsQueryable();
        }

        public void Update(TEntity entity)
        {
          if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                dbset.Update(entity);
                _context.SaveChanges();
            }
        }
    }
}
