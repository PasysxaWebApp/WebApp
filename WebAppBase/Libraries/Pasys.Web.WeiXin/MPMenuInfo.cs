using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class MPMenuInfo
    {
        /// <summary>
        /// 组织Id
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRoot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MenuKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MenuUrl { get; set; }

        public int Row { get; set; }

        public int Col { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DisplayFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UpdateID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public MPMenuInfo()
        {
            DisplayFlag = true;
            DeleteFlag = false;
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }
    }


}
