using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebAppBase.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "角色名称")]
        public string Name { get; set; }

        [Display(Name = "角色描述")]
        public string Description { get; set; }
    }

    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "电邮地址")]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> UserRoles { get; set; }

        public string RoleName { get; set; }

        public IEnumerable<SelectListItem> RolesSelectList { get; set; }

        public IEnumerable<RoleViewModel> RolesList { get; set; }
    }

    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public IEnumerable<RoleViewModel> Roles { get; set; }
        public IEnumerable<RoleViewModel> UserRoles { get; set; }
    }
}
