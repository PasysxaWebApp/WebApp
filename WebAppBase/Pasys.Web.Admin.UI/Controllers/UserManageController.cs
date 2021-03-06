﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Pasys.Web.Admin.UI.Models;
using Pasys.Web.Identity.Models;
using Pasys.Web.Identity;

namespace Pasys.Web.Admin.UI.Controllers
{
    public class UserManageController : Controller
    {
        private AppIdentityDbContext db = new AppIdentityDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: UserManager
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: UserManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user=db.Users.Find(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            if (userEditViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userEditViewModel);
        }

        // GET: UserManager/Create
        public ActionResult Create()
        {
            var rolesList = new List<Models.RoleViewModel>();
            var mdl = new Models.UserEditViewModel();
            db.Roles.ForEachAsync(item =>
            {
                rolesList.Add(new RoleViewModel{ Id=item.Id,Name=item.Name});
            });
            mdl.RolesList = rolesList;
            return View(mdl);
        }

        // POST: UserManager/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,UserRoles")] UserEditViewModel userEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userEditViewModel.Email, Email = userEditViewModel.Email};
                var result = await UserManager.CreateAsync(user,"123456");
                var addRolesResult = await UserManager.AddToRolesAsync(user.Id,userEditViewModel.UserRoles.ToArray());
                if (result.Succeeded && addRolesResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(userEditViewModel);
        }

        // GET: UserManager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            
            if (userEditViewModel == null)
            {
                return HttpNotFound();
            }
             
            userEditViewModel.UserRoles = UserManager.GetRolesAsync(id).Result.ToList();
            userEditViewModel.RolesList = GetRoleList();
            return View(userEditViewModel);
        }

        // POST: UserManager/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,UserRoles")] UserEditViewModel userEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(userEditViewModel.Id);
                user.Email = userEditViewModel.Email;
                user.UserName = userEditViewModel.Email;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                var userRoles = UserManager.GetRoles(user.Id).ToArray();
                UserManager.RemoveFromRoles(user.Id, userRoles);
                UserManager.AddToRoles(user.Id, userEditViewModel.UserRoles.ToArray());

                return RedirectToAction("Index");
            }
            return View(userEditViewModel);
        }

        // GET: UserManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            var userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                //RoleId = user.RoleId
            };
            
            if (userEditViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userEditViewModel);
        }

        // POST: UserManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<Models.RoleViewModel> GetRoleList()
        {
            var rolesList = new List<RoleViewModel>();
            db.Roles.ForEachAsync(item =>
            {
                rolesList.Add(new RoleViewModel { Id=item.Id,Name=item.Name });
            });
            return rolesList;
        }
    }
}
