using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.EntityManager
{

    /// <summary>
    ///     Used to validate an item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityValidator<T>
    {
        /// <summary>
        ///     Validate the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<ValidatorResult> ValidateAsync(T item);
    }



    /// <summary>
    ///     Validates entities before they are saved
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityValidatorBase<TEntity, TKey> : IEntityValidator<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected EntityManagerBase<TEntity, TKey> Manager { get; private set; }
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public EntityValidatorBase(EntityManagerBase<TEntity, TKey> manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            Manager = manager;
        }


        /// <summary>
        ///     Validates a entity before saving
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<ValidatorResult> ValidateAsync(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var errors = new List<string>();
            await ValidateEntity(item, errors).WithCurrentCulture();
            if (errors.Count > 0)
            {
                return ValidatorResult.Failed(errors.ToArray());
            }
            return ValidatorResult.Success;
        }

        protected virtual async Task ValidateEntity(TEntity entity, List<string> errors)
        {
            await Task.FromResult(0);
        }
    }

}
