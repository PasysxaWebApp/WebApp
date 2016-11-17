using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.EntityManager
{
    /// <summary>
    ///     Validates entities before they are saved
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class StringKeyEntityValidator<TEntity> : EntityValidatorBase<TEntity, string> where TEntity : class, IEntity<string>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public StringKeyEntityValidator(EntityManagerBase<TEntity, string> manager)
            : base(manager)
        {
        }
    }

    /// <summary>
    ///     Validates entities before they are saved
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class IntKeyEntityValidator<TEntity> : EntityValidatorBase<TEntity, int> where TEntity : class, IEntity<int>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public IntKeyEntityValidator(EntityManagerBase<TEntity, int> manager)
            : base(manager)
        {
        }
    }


}
