using Pasys.Web.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pasys.Core.EntityManager;
using Pasys.Web.Admin.UI.Models;
using System.Linq.Expressions;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class SampleController : EditableController<string, MemberCard.MemberCard, MemberCardViewModel, MemberCard.MemberCardManager>
    {
        public override GridListViewModel GetGridListViewModel()
        {
            var vm = new GridListViewModel() {
                DefaultSortColumn = "MemberCardId",
                LoadjqDataUrl = "LoadjqData",
                KeyColumn = "MemberCardId",
                NameColumn = "CardNo",
            };
            vm.Columns.Add(new GridListColumn() { Label = "CardNo", Name = "CardNo", Index = "CardNo", Width = "20%", Sortable = true,  Align = "center" });
            vm.Columns.Add(new GridListColumn() { Label = "MemberCardId", Name = "MemberCardId", Index = "index", Width = "10%", Sortable = true, Frozen = true, Align = "center" });
            vm.Columns.Add(new GridListColumn() { Label = "OrganizationId", Name = "OrganizationId", Index = "OrganizationId", Width = "30%", Sortable = true, Frozen = true, Align = "center" });
            vm.Columns.Add(new GridListColumn() { Label = "UserId", Name = "UserId", Index = "UserId", Width = "20%", Sortable = true, Align = "center" });
            vm.Columns.Add(new GridListColumn() { Label = "Balance", Name = "Balance", Index = "Balance", Width = "10%", Sortable = true, Align = "center" });
            vm.Columns.Add(new GridListColumn() { Label = "Status", Name = "Status", Index = "Status", Width = "10%", Sortable = true, Frozen = true, Align = "center" });
            return vm;
        }

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
        //public override ActionResult Index()
        //{
        //    var vm = GetGridListViewModel();
        //    return View(vm);
        //}

        #region jqGridData

        public override IQueryable<MemberCard.MemberCard> FilterData(IQueryable<MemberCard.MemberCard> list)
        {
            var searchText = this.Request["searchText"];
            if (!string.IsNullOrEmpty(searchText))
            {
                var rst = list.Where(s => s.CardNo.Contains(searchText));
                return rst;
            }
            else {
                return list;
            }

        }
        public override object GetJsonDataModel(MemberCard.MemberCard card)
        {
            return new
            {
                id = card.MemberCardId,
                cell = new object[] 
                {
                    card.MemberCardId,
                    card.OrganizationId,
                    card.UserId,
                    card.CardNo,
                    card.Balance ,
                    card.CashSum,
                }
            };
        }
        #endregion

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
            if (entity == null)
            {
                return null;
            }
            var model = new MemberCardViewModel()
            {
                MemberCardId=entity.MemberCardId,
                UserId = entity.UserId,
                CardNo = entity.CardNo,
                OrganizationId = entity.OrganizationId,
                ValidityTo=DateTime.Now
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