using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRepository<TEntity> where TEntity:class
    {
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public IQueryable<TEntity> GetAll();

        public IQueryable<TEntity> Find(Expression<Func<TEntity,bool>> expression,params Expression<Func<TEntity, Object>>[] includes);

    }
}
