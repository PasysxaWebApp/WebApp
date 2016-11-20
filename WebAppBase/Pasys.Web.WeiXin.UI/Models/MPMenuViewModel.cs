using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pasys.Web.WeiXin.UI.Models
{
    public class MPMenuViewModel
    {
        public string token { get; set; }
        public GetMenuResultFull ResultFull { get; set; }
        public MenuMatchRule MenuMatchRule { get; set; }
    }


}