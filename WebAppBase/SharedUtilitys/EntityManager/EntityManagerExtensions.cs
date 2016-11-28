using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.EntityManager
{
    /// <summary>
    ///     Extension methods for EntityManager
    /// </summary>
    public static class EntityManagerExtensions
    {
        /// <summary>
        ///     Find a entity by id
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static TEntity FindById<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, TKey entityId)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.FindByIdAsync(entityId));
        }


        /// <summary>
        ///     Create a entity
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ValidatorResult Create<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, TEntity entity)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.CreateAsync(entity));
        }

        /// <summary>
        ///     Update an existing entity
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ValidatorResult Update<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, TEntity entity)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.UpdateAsync(entity));
        }

        /// <summary>
        ///     Delete a entity
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ValidatorResult Delete<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, TEntity entity)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.DeleteAsync(entity));
        }
        public static ValidatorResult DeleteById<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, TKey id)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.DeleteByIdAsync(id));
        }

        /// <summary>
        ///     Find a entity by name
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static TEntity FindByName<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, string entityName)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.FindByNameAsync(entityName));
        }

        /// <summary>
        ///     Returns true if the entity exists
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static bool EntityExists<TEntity, TKey>(this EntityManagerBase<TEntity, TKey> manager, string entityName)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            return AsyncHelper.RunSync(() => manager.EntityExistsAsync(entityName));
        }
    }

}
