using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SnippetCache.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
                                                                                    IOrderedQueryable<TEntity>> orderBy
                                                                                    = null,
                                 string includeProperties = null);

        TEntity GetById(int id);
        void Insert(TEntity e);
        void Delete(int id);
        void Delete(TEntity e);
        void Update(TEntity e);
    }
}