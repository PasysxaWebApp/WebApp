using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public class ApplicationFunction
    {
        private string _functionName;
        private string _functionKey;
        public int FunctionId { get; set; }
        public string FunctionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_functionKey))
                {
                    return _functionKey;
                }
                return string.Format("{0}_{1}", ControllerName, ActionName);
            }
            set
            {
                _functionKey = value;
            }
        }

        public string FunctionName
        {
            get
            {
                if (!string.IsNullOrEmpty(_functionName))
                {
                    return _functionName;
                }
                return MenuName;
            }
            set
            {
                _functionName = value;
            }
        }
        public string MenuName { get; set; }
        public int ParentFunctionId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string CssClass { get; set; }
        public string ActionParam { get; set; }
        public bool IsRootMenu
        {
            get
            {
                return this.FunctionId == ParentFunctionId;
            }
        }
        public bool ShowInMenu { get; set; }
        public bool SeparateMenuFlag { get; set; }

        //public virtual ApplicationMenu ParentMenu { get; set; }

        //public virtual List<ApplicationMenu> SubMenus { get; set; }
        //public virtual List<ApplicationRoleMenu> RoleMenus { get; set; }
    }
}
