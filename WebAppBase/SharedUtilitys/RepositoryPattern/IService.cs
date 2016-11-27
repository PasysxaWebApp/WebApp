﻿using System;
using Pasys.Core.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using Pasys.Core.IOC;

namespace Pasys.Core.RepositoryPattern
{
    public interface IService<T> : IDependency where T : class
    {
        IRepository<T> Repository { get; set; }
        IApplicationContext ApplicationContext { get; set; }
        T Get(params object[] primaryKeys);
        IEnumerable<T> Get();
        IEnumerable<T> Get(DataFilter filter);
        IEnumerable<T> Get(Expression<Func<T, bool>> expression);
        IEnumerable<T> Get(DataFilter filter, Pagination pagin);
        IEnumerable<T> Get(string property, OperatorType operatorType, object value);
        void Add(T item);
        int Delete(params object[] primaryKeys);
        int Delete(DataFilter filter);
        int Delete(Expression<Func<T, bool>> expression);
        int Delete(T item);
        bool Update(T item, DataFilter filter);
        bool Update(T item, params object[] primaryKeys);
        long Count(DataFilter filter);
        long Count(Expression<Func<T, bool>> expression);
    }
}
