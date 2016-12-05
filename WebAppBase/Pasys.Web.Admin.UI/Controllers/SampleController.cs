using Pasys.Web.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pasys.Core.EntityManager;
using Pasys.Web.Admin.UI.Models;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class SampleController : EditableController<string, MemberCard.MemberCard, MemberCardViewModel, MemberCard.MemberCardManager>
    {

        public SampleController()
            : base(new MemberCard.MemberCardManager())
        { }

        private List<SelectListItem> getDeleteListItems()
        {
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem() { Text = "正常使用", Value = "1" });
            listItems.Add(new SelectListItem() { Text = "逻辑删除", Value = "2" });
            listItems.Add(new SelectListItem() { Text = "物理删除", Value = "3" });
            listItems.Add(new SelectListItem() { Text = "删数据库", Value = "4" });
            listItems.Add(new SelectListItem() { Text = "更换硬盘", Value = "5" });
            listItems.Add(new SelectListItem() { Text = "扔掉主机", Value = "6" });
            return listItems;
        }

        public ActionResult Test()
        {
            ViewBag.DeleteListItems = getDeleteListItems();
            var model = new Pasys.Web.Admin.UI.Models.TestModel();
            model.UserID = "3";
            model.CanDoList = new List<string> { "1", "3" };
            return View(model);
        }
        [HttpPost]
        public ActionResult Test(Pasys.Web.Admin.UI.Models.TestModel model)
        {
            ViewBag.DeleteListItems = getDeleteListItems();
            return View(model);
        }


        // GET: Sample
        public override ActionResult Index()
        {
            return View();
        }

        #region jqGridData

        /// <summary>
        /// お知らせ一覧データ取得
        /// </summary>
        public ActionResult LoadjqData(string searchText, string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString)
        {
            var list = new List<TestModel>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new TestModel()
                {
                    UserID = string.Format("UserID{0}", i),
                    UserName = string.Format("UserName{0}", i),
                    UserKana = string.Format("UserKana{0}", i),
                    MultiText = string.Format("MultiText{0}", i),
                    ClassIndex = i,
                    LastUpdateTime =DateTime.Now.AddMinutes(i),
                });
            }

            var listData = list.AsQueryable();

            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredData = listData;

            // Sort the student list
            //var sortedData = SortIQueryable<PatientInfo>(filteredData, "PatientKanaGroup", sord);

            // Calculate the total number of pages
            var totalRecords = list.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid
            var data = (from s in filteredData
                        select new
                        {
                            id = s.UserID,
                            cell = new object[] 
                            {
                                s.UserID,
                                string.Format("{0:yyyy年MM月dd日}", s.LastUpdateTime),
                                s.UserName,
                                s.UserKana,
                                s.MultiText,
                                s.ClassIndex,
                            }
                        }).ToArray();

            // Send the data to the jQGrid
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data//.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion


        //public override ActionResult Create()
        //{
        //    ViewBag.DeleteListItems = getDeleteListItems();
        //    var model = new Pasys.Web.Admin.UI.Models.TestModel();
        //    model.UserID = "3";
        //    model.CanDoList = new List<string> { "1", "3" };
        //    return View(model);
        //}

        protected override MemberCard.MemberCard ConvertFromModel(MemberCardViewModel model)
        {
            var entityModel = model;
            if (entityModel == null)
            {
                throw new ArgumentException();
            }
            var entity = new MemberCard.MemberCard()
            {
                MemberCardId = entityModel.MemberCardId,
                UserId = entityModel.UserId,
                CardNo = entityModel.CardNo,
                OrganizationId = entityModel.OrganizationId
            };
            return entity;
        }

        protected override MemberCardViewModel ConvertToModel(MemberCard.MemberCard entity)
        {
            var model = new MemberCardViewModel()
            {
                MemberCardId=entity.MemberCardId,
                UserId = entity.UserId,
                CardNo = entity.CardNo,
                OrganizationId = entity.OrganizationId
            };
            return model;
        }

        public override JsonResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                ids = "4613452f-2063-4427-ba32-9b0385c32ff5";
            }
            return base.Delete(ids);
        }
    }
}