using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Pasys.Web.Core.EntityManager;
using Pasys.Core.ViewPort.Grid;
using Pasys.Core.Models;

namespace Pasys.Web.Core.Controllers
{
    public interface IEditableController<TKey,TEntity,  TManager>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TManager : EntityManagerBase<TEntity, TKey>
    {
        ActionResult Index();
        ActionResult Create();
        ActionResult Create(TEntity entity);
        ActionResult Edit(TKey Id);
        ActionResult Edit(TEntity entity);
        JsonResult Delete(string ids);
        JsonResult GetList();
    }

    //public class EditableController2<TKey,TEntity, TManager> : System.Web.Mvc.Controller
    //    where TKey : IEquatable<TKey>
    //    where TEntity : class, IEntity<TKey>
    //    where TManager : EntityManagerBase<TEntity, TKey>
    //{ }


    public class EditableController<TKey,TEntity,  TManager> : System.Web.Mvc.Controller, IEditableController< TKey, TEntity,TManager>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TManager : EntityManagerBase<TEntity, TKey>
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
            //string filePath = Request.SaveImage();
            //if (!string.IsNullOrEmpty(filePath))
            //{
            //    entity.ImageUrl = filePath;
            //    string fileName = ImageUnity.SetThumb(Server.MapPath(filePath), ImageThumbWidth ?? 64, ImageThumbHeight ?? 64);
            //    entity.ImageThumbUrl = filePath.Replace(System.IO.Path.GetFileName(filePath), fileName);
            //}
        }

        public virtual ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult Create()
        {
            var entity = Activator.CreateInstance<TEntity>();
            return View(entity);
        }
        [HttpPost]
        public virtual ActionResult Create(TEntity entity)
        {
            if (ModelState.IsValid)
            {
                UpLoadImage(entity as IImage);
                EntityManager.Create(entity);
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        public virtual ActionResult Edit(TKey Id)
        {
            var entity = EntityManager.FindById(Id);
            return View(entity);
        }
        [HttpPost]
        public virtual ActionResult Edit(TEntity entity)
        {
            if (ModelState.IsValid)
            {
                UpLoadImage(entity as IImage);
                EntityManager.Update(entity);
                return RedirectToAction("Index");
            }
            return View(entity);
        }
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
    }

}
