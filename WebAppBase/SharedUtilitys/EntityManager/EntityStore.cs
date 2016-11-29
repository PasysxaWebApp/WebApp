using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core.EntityManager
{    
    /// <summary>
    ///     EntityFramework based implementation
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserRole"></typeparam>
    public class EntityStore<TEntity, TKey> : IQueryableIEntityStore<TEntity, TKey>
        where TEntity :class, IEntity<TKey>, new()
    {
        private bool _disposed;
        private BaseEntityStore<TEntity> _entityStore;

        /// <summary>
        ///     Constructor which takes a db context and wires up the stores with default instances using the context
        /// </summary>
        /// <param name="context"></param>
        public EntityStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            Context = context;
            _entityStore = new BaseEntityStore<TEntity>(context);
        }

        /// <summary>
        ///     Context for the store
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        ///     If true will call dispose on the DbContext during Dipose
        /// </summary>
        public bool DisposeContext { get; set; }

        /// <summary>
        ///     Find a entity by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Task<TEntity> FindByIdAsync(TKey entityId)
        {
            ThrowIfDisposed();
            return _entityStore.GetByIdAsync(entityId);
        }

        /// <summary>
        ///     Find a entity by name
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public Task<TEntity> FindByNameAsync(string entityName)
        {
            ThrowIfDisposed();
            return _entityStore.EntitySet.FirstOrDefaultAsync(u =>u.EntityName.ToUpper() == entityName.ToUpper());
        }

        /// <summary>
        ///     Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task CreateAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entityStore.Create(entity);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        ///     Mark an entity for deletion
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entityStore.Delete(entity);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }
        public virtual async Task DeleteByIdAsync(TKey id)
        {
            ThrowIfDisposed();
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            _entityStore.DeleteById(id);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }

        public virtual bool DeleteByEntities(List<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            var tran = Context.Database.BeginTransaction();
            try
            {
                _entityStore.Delete(entities);
                Context.SaveChanges();
                tran.Commit();
                return true;
            }
            catch (Exception)
            {
                tran.Rollback();
                return false;
            }
        }

        public virtual bool DeleteByIds(IEnumerable<TKey> ids)
        {
            ThrowIfDisposed();
            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }
            var tran = Context.Database.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    _entityStore.DeleteById(id);
                }
                Context.SaveChanges();
                tran.Commit();
                return true;
            }
            catch (Exception)
            {
                tran.Rollback();
                return false;
            }
        }


        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entityStore.Update(entity);
            await Context.SaveChangesAsync().WithCurrentCulture();
        }

        /// <summary>
        ///     Returns an IQueryable of entities
        /// </summary>
        public IQueryable<TEntity> Entities
        {
            get { return _entityStore.EntitySet; }
        }

        /// <summary>
        ///     Dispose the store
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        ///     If disposing, calls dispose on the Context.  Always nulls out the Context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (DisposeContext && disposing && Context != null)
            {
                Context.Dispose();
            }
            _disposed = true;
            Context = null;
            _entityStore = null;
        }
    }

}
