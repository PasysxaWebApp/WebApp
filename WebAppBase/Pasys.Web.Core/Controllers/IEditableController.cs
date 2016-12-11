using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Pasys.Core.EntityManager;
using Pasys.Core.ViewPort.Grid;
using Pasys.Core.Models;
using Pasys.Web.Core.Attributes;

namespace Pasys.Web.Core.Controllers
{
    public interface IEditableController<TKey,TEntity,  TManager>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TManager : IEntityManager<TEntity, TKey>
    {
        ActionResult Index();
        ActionResult Create();
        ActionResult Create(object entity);
        ActionResult Edit(TKey Id);
        ActionResult Edit(object entity);
        JsonResult Delete(string ids);
        JsonResult GetList();
    }

    //public class EditableController2<TKey,TEntity, TManager> : System.Web.Mvc.Controller
    //    where TKey : IEquatable<TKey>
    //    where TEntity : class, IEntity<TKey>
    //    where TManager : EntityManagerBase<TEntity, TKey>
    //{ }


    public class EditableController<TKey,TEntity,TViewModel, TManager> : System.Web.Mvc.Controller, IEditableController< TKey, TEntity,TManager>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>,new()
        where TViewModel : class, new()
        where TManager : IEntityManager<TEntity, TKey>
    {
        /// <summary>
        /// 缩略图宽
        /// </summary>
        public int? ImageThumbWidth { get; set; }
        /// <summary>
        /// 缩略图高
        /// </summary>
        public int? ImageThumbHeight { get; set; }
        /// <summary>
        /// 业务Service
        /// </summary>
        public TManager EntityManager;
        public EditableController(TManager service)
        {
            EntityManager = service;
        }

        protected void UpLoadImage(IImage entity)
        {
            if (entity == null) return;
            if (!string.IsNullOrEmpty(entity.ImageUrl) && string.IsNullOrEmpty(entity.ImageThumbUrl))
            {
                entity.ImageThumbUrl = entity.ImageUrl;
            }
        }

        public virtual ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult Create()
        {
            var entity = Activator.CreateInstance<TEntity>();
            var model = ConvertToModel(entity);
            ViewBag.Title = GetTitle();
            ViewBag.SubTitle = GetSubTitle();
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Create(object obj)
        {
            var model = BindModel();
            if (ModelState.IsValid)
            {
                var editEntity = ConvertFromModel(model);
                UpLoadImage(model as IImage);
                EntityManager.Create(editEntity);
                return RedirectToAction("Index");
            }
            ViewBag.Title = GetTitle();
            ViewBag.SubTitle = GetSubTitle();
            return View(model);
        }

        public virtual ActionResult Edit(TKey Id)
        {
            var entity = EntityManager.FindById(Id);
            var model = ConvertToModel(entity);
            ViewBag.Title = GetTitle();
            ViewBag.SubTitle = GetSubTitle();
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Edit(object obj)
        {
            var model = BindModel();
            if (ModelState.IsValid)
            {
                var editEntity = ConvertFromModel(model);
                UpLoadImage(model as IImage);
                EntityManager.Update(editEntity);
                return RedirectToAction("Index");
            }
            ViewBag.Title = GetTitle();
            ViewBag.SubTitle = GetSubTitle();
            return View(model);
        }
        [HttpPost]
        public virtual JsonResult Delete(string ids)
        {
            try
            {
                string[] idSplit = ids.Split(',');
                var tIds = new List<TKey>();
                foreach (var id in idSplit)
                {
                    TKey tId = (TKey)Convert.ChangeType(id, typeof(TKey));
                    tIds.Add(tId);
                }
                var result = EntityManager.DeleteByIds(tIds);
                if (result)
                {
                    return Json(new AjaxResult { Status = AjaxStatus.Normal, Message = ids });
                }
                else
                {
                    return Json(new AjaxResult { Status = AjaxStatus.Warn, Message = "未删除任何数据！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { Status = AjaxStatus.Error, Message = ex.Message });
            }
        }
        public virtual JsonResult GetList()
        {
            throw new NotImplementedException();
        }

        protected virtual TEntity ConvertFromModel(TViewModel model)
        {
            throw new NotImplementedException();
        }

        protected virtual TViewModel ConvertToModel(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected virtual TViewModel BindModel()
        {
            var model = new TViewModel();
            var bl = this.TryUpdateModel(model);
            if (!bl)
            {
                throw new ArgumentException();
            }
            return model;
        }

        protected virtual string GetTitle()
        {
            var t = typeof(TViewModel);
            var vmAttr = t.GetViewModelAttribute();
            if (vmAttr != null)
            {
                return vmAttr.Title;
            }
            else {
                return string.Empty;
            }
        }
        protected virtual string GetSubTitle()
        {
            var t = typeof(TViewModel);
            var vmAttr = t.GetViewModelAttribute();
            if (vmAttr != null)
            {
                return vmAttr.SubTitle;
            }
            else
            {
                return string.Empty;
            }
        }
    }

}
