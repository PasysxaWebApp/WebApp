﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    // ApplicationUser クラスにプロパティを追加することでユーザーのプロファイル データを追加できます。詳細については、http://go.microsoft.com/fwlink/?LinkID=317594 を参照してください。
    public class ApplicationUser : IdentityUser
    {
        private  string _nice_name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // ここにカスタム ユーザー クレームを追加します
            return userIdentity;
        }
        public virtual string OrganizationId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NiceName
        {
            get
            {
                if (!string.IsNullOrEmpty(_nice_name))
                {
                    return _nice_name;
                }
                else {
                    return this.UserName;
                }
            }
            set
            {
                _nice_name = value;
            }
        }
        //public virtual List<ApplicationRoleMenu> RoleMenus { get; set; }
        public virtual ApplicationOrganization Organization { get; set; }
    }
}
