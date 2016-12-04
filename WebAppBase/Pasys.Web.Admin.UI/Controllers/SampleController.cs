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