using Pasys.Core.MetaData;
using Pasys.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Admin.UI.Models
{

    [DataConfigure(typeof(SampleModelMeterData))]
    public class SampleModel:IEntity<string>
    {
        [Display(Name = "角色ID")]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "角色名称")]
        public string EntityName { get; set; }
        [Display(Name = "角色描述")]
        public string Description { get; set; }
        
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
            //var multiSelect = ViewConfig(m => m.MultiSelect).AsDropDownList();
            //multiSelect.SourceType = Pasys.Core.Constant.SourceType.Dictionary;
            //multiSelect.DataSource(dic);
        }
    }

    public class SampleModelStore : DefaultModelStore<SampleModel, string>
    {
        public SampleModelStore(DbContext context) : base(context) { }

    }

    public class SampleModelManager : DefaultModelManager<SampleModel, string>
    {
       
    }


}
