using Pasys.Core.Data;
using System.Collections.Generic;
using Pasys.Core.Data.DataBase;
using Pasys.Core.IOC;

namespace Pasys.Core.RepositoryPattern
{
    public interface IRepository<T> : IDependency where T : class
    {
        DataBasic DataBase { get; set; }
        IApplicationContext ApplicationContext { get; set; }
        void Add(T item);
        int Delete(DataFilter filter);
        int Delete(params object[] primaryKeys);
        int Delete(T item);IEnumerable<T> Get(DataFilter filter);
        IEnumerable<T> Get(DataFilter filter, Pagination pagin);
        T Get(params object[] primaryKeys);
        bool Update(T item, DataFilter filter);
        bool Update(T item, params object[] primaryKeys);
        long Count(DataFilter filter);
    }
}
