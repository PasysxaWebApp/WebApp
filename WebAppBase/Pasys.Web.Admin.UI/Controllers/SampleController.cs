using Pasys.Web.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pasys.Web.Core.EntityManager;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class SampleController : EditableController<string, MemberCard.MemberCard, MemberCard.MemberCardManager>
    {

        public SampleController()
            : base(new MemberCard.MemberCardManager())
        { }

        // GET: Sample
        public override ActionResult Index()
        {
            return View();
        }

        public override ActionResult Create()
        {
            return base.Create();
        }

        public override ActionResult Create(MemberCard.MemberCard entity)
        {
            return base.Create(entity);
        }

        public override ActionResult Edit(string Id)
        {
            return base.Edit(Id);
        }

        public override ActionResult Edit(MemberCard.MemberCard entity)
        {
            return base.Edit(entity);
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