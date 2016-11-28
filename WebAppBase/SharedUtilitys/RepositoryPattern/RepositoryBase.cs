using Pasys.Core.Data;
using Pasys.Core.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Core.RepositoryPattern
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        public DataBasic DataBase { get; set; }
        public IApplicationContext ApplicationContext { get; set; }
        public void Add(T item)
        {
            throw new NotImplementedException();
        }
        public int Delete(DataFilter filter)
        {
            throw new NotImplementedException();
        }
        public int Delete(params object[] primaryKeys)
        {
            throw new NotImplementedException();
        }
        public int Delete(T item)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<T> Get(DataFilter filter)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<T> Get(DataFilter filter, Pagination pagin)
        {
            throw new NotImplementedException();
        }
        public T Get(params object[] primaryKeys)
        {
            throw new NotImplementedException();
        }
        public bool Update(T item, DataFilter filter)
        {
            throw new NotImplementedException();
        }
        public bool Update(T item, params object[] primaryKeys)
        {
            throw new NotImplementedException();
        }
        public long Count(DataFilter filter)
        {
            throw new NotImplementedException();
        }

    }
}
