using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public class ApplicationOrganization
    {
        public string OrganizationID { get; set; }
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 利用状态
        /// </summary>
        public int Status { get; set; }
        public string OfficeName { get; set; }
        public string StaffName { get; set; }
        public string Postal { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string MailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LastUserID { get; set; }
        public DateTime LastUpdatetime { get; set; }

        //public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<ApplicationRole> Roles { get; set; }

    }
}
