using Pasys.Core.Models;
using Pasys.Web.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pasys.Web.Core
{
    public interface IWorkContext 
    {
        string Area { get; }
    }

    public interface IWorkController<T> where T : IWorkContext
    {
        T WorkContext { get; }
    }

    public interface IEditableController<TEntity,TKey,TManager>
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>
        where TManager : EntityManagerBase<IEntity<TKey>, TKey>
    {
        ActionResult Index();
        ActionResult Create();
        ActionResult Create(TEntity entity);
        ActionResult Edit(TKey Id);
        ActionResult Edit(TEntity entity);
        JsonResult Delete(string ids);
        JsonResult GetList();
    }
}
