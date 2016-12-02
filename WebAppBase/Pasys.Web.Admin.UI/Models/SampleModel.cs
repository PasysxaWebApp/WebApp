using Pasys.Core.MetaData;
using Pasys.Core.EntityManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pasys.Web.Core.Attributes;
using System.Web.Mvc;
using System.ComponentModel;

namespace Pasys.Web.Admin.UI.Models
{

    public class TestModel 
    {
        [DisplayName("ユーザー")]
        [ViewDataSource]
        public string UserID { get; set; }

        [TextBoxDataSource(ResourceType = typeof(Resources.LanguageResource), PlaceHolder = "PlaceholderUserName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "PlaceholderUserName")]
        public string UserName { get; set; }

        [DisplayName("ユーザーカナ")]
        public string UserKana { get; set; }

        [DisplayName("クラス")]
        [ClassDataSource]
        public int ClassIndex { get; set; }

        [DisplayName("削除タップ")]
        [DeleteDataSource]
        public int? DelFlag { get; set; }

        [DisplayName("表示")]
        [YesNoDataSource]
        public int? DisFlag { get; set; }

        [DisplayName("我能")]
        [DeleteCheckBoxDataSource]
        public List<string> CanDoList { get; set; }
        [TextAreaDataSource]
        public string MultiText { get; set; }
    }


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

    class ViewDataSourceAttribute : DropDownDataSourceAttributeBase
    {

        public override SelectList GetData(object selectedValue)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(new SelectListItem() { Value = i.ToString(), Text = "item" + i });
            }
            var lst = new SelectList(items, "Value", "Text", selectedValue);
            return lst;
        }
    }

    class ClassDataSourceAttribute : DropDownDataSourceAttributeBase
    {
        public override SelectList GetData(object selectedValue)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(new SelectListItem() { Value = i.ToString(), Text = "Class" + i });
            }
            var lst = new SelectList(items, "Value", "Text", selectedValue);
            return lst;
        }
    }


    class YesNoDataSourceAttribute : RadioButtonDataSourceAttributeBase
    {
        private string[] yesStrs = { "yes", "true", "1" };
        public override SelectList GetData(object selectedValue)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = "1", Text = "Yes" });
            items.Add(new SelectListItem() { Value = "0", Text = "No" });
            string s = string.Format("{0}", selectedValue).ToLower();
            var sv = "0";
            if (yesStrs.Contains(s))
            {
                sv = "1";
            }
            var lst = new SelectList(items, "Value", "Text", sv);
            return lst;
        }
    }

    class DeleteDataSourceAttribute : RadioButtonDataSourceAttributeBase
    {
        public override SelectList GetData(object selectedValue)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "正常使用", Value = "1" });
            items.Add(new SelectListItem() { Text = "逻辑删除", Value = "2" });
            items.Add(new SelectListItem() { Text = "物理删除", Value = "3" });
            var lst = new SelectList(items, "Value", "Text", selectedValue);
            return lst;
        }
    }

    class DeleteCheckBoxDataSourceAttribute : CheckBoxDataSourceAttributeBase
    {
        public override SelectList GetData(object selectedValue)
        {
            var svs = selectedValue as List<string>;// string.Format("{0}", selectedValue).Split(new char[] { ',' });
            if (svs == null)
            {
                svs = new List<string>();
            }

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "正常使用", Value = "1" });
            items.Add(new SelectListItem() { Text = "逻辑删除", Value = "2" });
            items.Add(new SelectListItem() { Text = "物理删除", Value = "3" });
            items.Add(new SelectListItem() { Text = "删库", Value = "4" });
            items.Add(new SelectListItem() { Text = "换硬盘", Value = "5" });
            items.Add(new SelectListItem() { Text = "挪主机", Value = "6" });

            foreach (var item in items)
            {
                item.Selected = svs.Contains(item.Value);
            }

            var lst = new SelectList(items, "Value", "Text");
            return lst;
        }
    }



}
