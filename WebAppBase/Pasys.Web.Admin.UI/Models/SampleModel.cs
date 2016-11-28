using Pasys.Core.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Admin.UI.Models
{

    [DataConfigure(typeof(SampleModelMeterData))]
    public class SampleModel
    {
        [Display(Name = "角色ID")]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "角色名称")]
        public string Name { get; set; }
        [Display(Name = "角色描述")]
        public string Description { get; set; }

        public bool ItemA { get; set; }
        public bool ItemB { get; set; }
        public bool ItemC { get; set; }
        public bool ItemD { get; set; }
        public bool ItemE { get; set; }

        public string MultiSelect { get; set; }


    }

    class SampleModelMeterData : DataViewMetaData<SampleModel>
    {

        protected override void DataConfigure()
        {
            //DataTable("ExampleItem");
            //DataConfig(m => m.ID).AsIncreasePrimaryKey();
        }

        protected override void ViewConfigure()
        {
            ViewConfig(m => m.Id).AsHidden();
            var dic = new Dictionary<string, string>();
            dic.Add("A", "A");
            dic.Add("B", "B");
            dic.Add("C", "C");
            dic.Add("D", "D");
            var multiSelect = ViewConfig(m => m.MultiSelect).AsDropDownList();
            multiSelect.SourceType = Pasys.Core.Constant.SourceType.Dictionary;
            multiSelect.DataSource(dic);
        }
    }
}
