using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core.EntityManager
{

    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext()
            : base("DefaultConnection")
        {
        }

        public static DefaultDbContext Create()
        {
            return new DefaultDbContext();
        }

    }

    public class DefaultModelStore<TEntity, TKey> : EntityStore<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()

    {
        public DefaultModelStore(DbContext context) : base(context) { }
    }



    public class DefaultModelManager<TEntity, TKey> : EntityManagerBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
        where TKey : IEquatable<TKey>

    {
        public DefaultModelStore<TEntity, TKey> BizStore
        {
            get
            {
                return (DefaultModelStore<TEntity, TKey>)this.Store;
            }
        }


        public DefaultModelManager()
            : this(DefaultDbContext.Create())
        { }

        public DefaultModelManager(DefaultDbContext dbContext)
            : this(new DefaultModelStore<TEntity, TKey>(dbContext))
        {
        }

        public DefaultModelManager(DefaultModelStore<TEntity, TKey> store)
            : base(store)
        {
            //this.EntityValidator = new MPEntityValidator(this);

        }
    }

}
