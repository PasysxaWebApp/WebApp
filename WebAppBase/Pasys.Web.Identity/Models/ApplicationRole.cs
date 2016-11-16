using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole( string organizationID, string name, string description) : base(name)
        {
            this.OrganizationID = organizationID;
            this.Description = description; 
        }

        public virtual string OrganizationID { get; set; }
        public virtual string Description { get; set; }

        public virtual List<ApplicationRoleMenu> RoleMenus { get; set; }
        public virtual ApplicationOrganization Organization { get; set; }
    }
}
