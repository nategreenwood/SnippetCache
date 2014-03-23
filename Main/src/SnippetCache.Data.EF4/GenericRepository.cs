using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using SnippetCache.Data.EF4.Model;

namespace SnippetCache.Data.EF4
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly SnippetCacheEntitiesContainer _context;
        private readonly ObjectSet<TEntity> _dbSet;

        public GenericRepository(SnippetCacheEntitiesContainer context)
        {
            _context = context;
            _dbSet = _context.CreateObjectSet<TEntity>();
        }

        #region IRepository<TEntity> Members

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                query = includeProperties
                    .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual TEntity GetById(int id)
        {
            return _dbSet.SingleOrDefault(d => d.Id == id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.AddObject(entity);
        }

        public virtual void Delete(int entityId)
        {
            TEntity entityToDelete = _dbSet.FirstOrDefault(d => d.Id == entityId);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_dbSet.Context.ObjectStateManager.GetObjectStateEntry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.DeleteObject(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            //_dbSet.Attach(entityToUpdate);
            _dbSet.Context.ObjectStateManager.GetObjectStateEntry(entityToUpdate).SetModified();
        }

        #endregion
    }
}