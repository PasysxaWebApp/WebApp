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
        public GridListViewModel GridListModel = new GridListViewModel();
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
        public virtual GridListViewModel GetGridListViewModel()
        {
            var vm = new GridListViewModel();
            return vm;
        }

        public virtual ActionResult Index()
        {
            SetViewBagTitle();
            var vm = GetGridListViewModel();
            ViewBag.SearchText = this.Request["SearchText"];
            return View("CommonIndex",vm);
        }

        /// <summary>
        /// Load data for jqgrid
        /// </summary>
        public ActionResult LoadjqData(string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString)
        {
            var list = from s in this.EntityManager.Entities select s;
            list = FilterData(list);
            sidx = sidx ?? this.GridListModel.DefaultSortColumn;
            list = list.OrderBy(s => sidx);

            var filteredData = list.Skip((page - 1) * rows);
            filteredData = filteredData.Take(rows);
            var resulltData = filteredData.ToList();

            // Calculate the total number of pages
            var totalRecords = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);
            // Prepare the data to fit the requirement of jQGrid
            var datas = filteredData.Select(ConvertToModel).Select(GetJsonDataModel);

            // Send the data to the jQGrid
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = datas//.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public virtual IQueryable<TEntity> FilterData(IQueryable<TEntity> list)
        {
            return list;
        }
        public virtual object GetJsonDataModel(TViewModel card)
        {
            throw new NotImplementedException();
        }


        public virtual ActionResult Create()
        {
            var entity = Activator.CreateInstance<TEntity>();
            var model = ConvertToModel(entity);
            SetViewBagTitle();
            return View("CommonCreate", model);
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
            SetViewBagTitle();
            return View("CommonCreate", model);
        }

        public virtual ActionResult Edit(TKey Id)
        {
            var entity = EntityManager.FindById(Id);
            var model = ConvertToModel(entity);
            if (model == null)
            {
                return new HttpNotFoundResult();
            }
            SetViewBagTitle();
            return View("CommonEdit", model);
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
            SetViewBagTitle();
            return View("CommonEdit", model);
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

        /*
        protected virtual string GetTitle()
        {
            var t = typeof(TViewModel);
            var vmAttr = t.GetViewModelAttribute();
            if (vmAttr != null)
            {
                return vmAttr.EditTitle;
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
                return vmAttr.EditSubTitle;
            }
            else
            {
                return string.Empty;
            }
        }
        */

        protected virtual void SetViewBagTitle()
        {
            var t = typeof(TViewModel);
            var vmEditAttr = t.GetEditViewModelAttribute();
            if (vmEditAttr != null)
            {
                ViewBag.EditTitle = vmEditAttr.Title;
                ViewBag.EditSubTitle = vmEditAttr.SubTitle;
            }
            var vmListAttr = t.GetListViewModelAttribute();
            if (vmListAttr != null)
            {
                ViewBag.ListTitle = vmListAttr.Title;
                ViewBag.ListSubTitle = vmListAttr.SubTitle;
            }
        }
    }

    public class GridListViewModel
    {
        public string DefaultSortColumn { get; set; }
        public string KeyColumn { get; set; }
        public string NameColumn { get; set; }

        public string LoadjqDataUrl { get; set; }

        public List<GridListColumn> Columns { get; set; }

        public GridListViewModel()
        {
            Columns = new List<GridListColumn>();
            LoadjqDataUrl = "LoadjqData";
        }

        public string[] GetColNames()
        {
            return Columns.Select(m => m.Label).ToArray();
        }

        public List<Dictionary<string, object>> GetColModels()
        {
            var list = new List<Dictionary<string, object>>();
            var dic = new Dictionary<string, object>();
            foreach (var d in Columns)
            {
                if (d.Name.Equals(this.NameColumn))
                {
                    d.Formatter = "jqGrid_NameEditLink";
                }
                list.Add(d.GetSetting());
            }
            return list;
        }

    }

    public class GridListColumn
    {
        private Dictionary<string, object> Settings = new Dictionary<string, object>();
        public string Label
        {
            get
            {
                return GetSettingAsString("label");
            }
            set
            {
                AddSetting("label", value);
            }
        }
        public string Name
        {
            get
            {
                return GetSettingAsString("name");
            }
            set
            {
                AddSetting("name", value);
            }
        }
        public string Index
        {
            get
            {
                return GetSettingAsString("index");
            }
            set
            {
                AddSetting("index", value);
            }
        }
        public string Width
        {
            get
            {
                return GetSettingAsString("width");
            }
            set
            {
                AddSetting("width", value);
            }
        }
        public bool Sortable
        {
            get
            {
                return GetSettingAsBool("sortable");
            }
            set
            {
                AddSetting("sortable", value);
            }
        }

        public bool Hidden
        {
            get
            {
                return GetSettingAsBool("hidden");
            }
            set
            {
                AddSetting("hidden", value);
            }
        }
        public bool Frozen
        {
            get
            {
                return GetSettingAsBool("frozen");
            }
            set
            {
                AddSetting("frozen", value);
            }
        }
        public string Align
        {
            get
            {
                return GetSettingAsString("align");
            }
            set
            {
                AddSetting("align", value);
            }
        }
        public string Formatter
        {
            get
            {
                return GetSettingAsString("formatter");
            }
            set
            {
                AddSetting("formatter", value);
            }
        }

        public void AddSetting(string settingKey, object settingValue)
        {
            if (string.IsNullOrEmpty(settingKey)) {
                return;
            }

            if (settingValue==null)
            {
                return;
            }
            var k = settingKey;//.ToLower();
            if (Settings.ContainsKey(k))
            {
                Settings[k] = settingValue;
            }
            else {
                Settings.Add(k, settingValue);
            }
        }
        //public void AddSetting(string settingKey, bool settingValue)
        //{
        //    AddSetting(settingKey, settingValue.ToString());
        //}
        public object GetSetting(string settingKey)
        {
            if (string.IsNullOrEmpty(settingKey))
            {
                return null;
            }

            var k = settingKey.ToLower();
            if (Settings.ContainsKey(k))
            {
                return Settings[k];
            }
            else
            {
                return null;
            }
        }

        public bool GetSettingAsBool(string settingKey)
        {
            var sv = GetSetting(settingKey);
            if (sv==null)
            {
                return false;
            }
            var trueValues = new[] { "true","1" };
            return trueValues.Contains(sv);
        }
        public string GetSettingAsString(string settingKey)
        {
            var sv = GetSetting(settingKey);
            if (sv == null)
            {
                return null;
            }
            return sv.ToString();
        }

        public Dictionary<string, object> GetSetting()
        {
            return Settings;
        }

        //public static string GetJavascriptKey(string k)
        //{
        //    if (string.IsNullOrEmpty(k))
        //    {
        //        return k;
        //    }
        //    if (k.Length == 1)
        //    {
        //        return k.ToLower();
        //    }
        //    return string.Format("{0}{1}", k.Substring(0, 1).ToLower(), k.Substring(1));
        //}

    }
}
