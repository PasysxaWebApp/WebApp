using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core.EntityManager
{
    public interface IEntity<TKey> : Pasys.Core.IOC.IEntity
    {
        string EntityName { get; }
    }

    public interface IEntityStore<TEntity, TKey> : IDisposable
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteByIdAsync(TKey id);
        Task DeleteAsync(TEntity entity);
        bool DeleteByEntities(List<TEntity> entities);
        bool DeleteByIds(IEnumerable<TKey> ids);
        Task<TEntity> FindByIdAsync(TKey key);
        Task<TEntity> FindByNameAsync(string key);
    }

    public interface IEntityManager<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        IQueryable<TEntity> Entities { get; }
        Task<ValidatorResult> CreateAsync(TEntity entity);
        Task<ValidatorResult> UpdateAsync(TEntity entity);
        Task<ValidatorResult> DeleteAsync(TEntity entity);
        Task<ValidatorResult> DeleteByIdAsync(TKey id);
        bool DeleteByEntities(List<TEntity> entities);
        bool DeleteByIds(IEnumerable<TKey> ids);
        Task<TEntity> FindByIdAsync(TKey entityId);
        Task<bool> EntityExistsAsync(string entityName);
        Task<TEntity> FindByNameAsync(string entityName);

    }

    ///// <summary>
    /////     Interface that exposes an IQueryable entitys
    ///// </summary>
    ///// <typeparam name="TEntity"></typeparam>
    //public interface IQueryableEntityStore<TEntity> : IQueryableIEntityStore<TEntity, string> where TEntity : IEntity<string>
    //{
    //}

    /// <summary>
    ///     Interface that exposes an IQueryable entitys
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IQueryableIEntityStore<TEntity, TKey> : IEntityStore<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        ///     IQueryable Entities
        /// </summary>
        IQueryable<TEntity> Entities { get; }
    }


    public abstract class EntityManagerBase<TEntity, TKey> :IEntityManager<TEntity, TKey>,  IDisposable
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private bool _disposed;
        //private IEntityValidator<TEntity> _entityValidator;
        protected virtual IEntityStore<TEntity, TKey> Store { get; private set; }

        public EntityManagerBase(IEntityStore<TEntity, TKey> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }
            Store = store;
            //EntityValidator = new IEntityValidator<TEntity>(this);
        }


        /// <summary>
        ///     Used to validate entitys before persisting changes
        /// </summary>
        //public IEntityValidator<TEntity> EntityValidator
        //{
        //    get { return _entityValidator; }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            throw new ArgumentNullException("value");
        //        }
        //        _entityValidator = value;
        //    }
        //}


        /// <summary>
        ///     Returns an IQueryable of entitys if the store is an IQueryableRoleStore
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get
            {
                var queryableStore = Store as IQueryableIEntityStore<TEntity, TKey>;
                if (queryableStore == null)
                {
                    throw new NotSupportedException();
                }
                return queryableStore.Entities;
            }
        }

        /// <summary>
        ///     Dispose this object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Create a entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ValidatorResult> CreateAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            //var result = await EntityValidator.ValidateAsync(entity).WithCurrentCulture();
            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            await Store.CreateAsync(entity).WithCurrentCulture();
            return ValidatorResult.Success;
        }

        /// <summary>
        ///     Update an existing entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ValidatorResult> UpdateAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            //var result = await EntityValidator.ValidateAsync(entity).WithCurrentCulture();
            //if (!result.Succeeded)
            //{
            //    return result;
            //}
            await Store.UpdateAsync(entity).WithCurrentCulture();
            return ValidatorResult.Success;
        }

        /// <summary>
        ///     Delete a entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ValidatorResult> DeleteAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            await Store.DeleteAsync(entity).WithCurrentCulture();
            return ValidatorResult.Success;
        }
        public virtual async Task<ValidatorResult> DeleteByIdAsync(TKey id)
        {
            ThrowIfDisposed();
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            await Store.DeleteByIdAsync(id).WithCurrentCulture();
            return ValidatorResult.Success;
        }

        //public virtual async Task<ValidatorResult> DeleteAsync(TEntity entity)
        //{
        //    ThrowIfDisposed();
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException("entity");
        //    }

        //    await Store.DeleteAsync (entity).WithCurrentCulture();
        //    return ValidatorResult.Success;
        //}

        public bool DeleteByEntities(List<TEntity> entities)
        {
            ThrowIfDisposed();
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return Store.DeleteByEntities(entities);
        }
        public bool DeleteByIds(IEnumerable<TKey> ids)
        {
            ThrowIfDisposed();
            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }

            return Store.DeleteByIds(ids);
        }

        /// <summary>
        ///     Find a entity by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindByIdAsync(TKey entityId)
        {
            ThrowIfDisposed();
            return await Store.FindByIdAsync(entityId).WithCurrentCulture();
        }

        /// <summary>
        ///     Returns true if the entity exists
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual async Task<bool> EntityExistsAsync(string entityName)
        {
            ThrowIfDisposed();
            if (entityName == null)
            {
                throw new ArgumentNullException("entityName");
            }

            return await FindByNameAsync(entityName).WithCurrentCulture() != null;
        }

        /// <summary>
        ///     Find a entity by name
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindByNameAsync(string entityName)
        {
            ThrowIfDisposed();
            if (entityName == null)
            {
                throw new ArgumentNullException("entityName");
            }

            return await Store.FindByNameAsync(entityName).WithCurrentCulture();
        }


        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        ///     When disposing, actually dipose the store
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store.Dispose();
            }
            _disposed = true;
        }

    }
}
