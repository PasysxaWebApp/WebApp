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

    public class TestModel : IEntity
    {
        [HiddenInput(DisplayValue=false)]
        [DisplayName("ユーザー")]
        public string UserID { get; set; }

        [TextBoxDataSource(ResourceType = typeof(Resources.LanguageResource), PlaceHolder = "PlaceholderUserName")]
        [Required(ErrorMessageResourceType = typeof(Resources.LanguageResource), ErrorMessageResourceName = "PlaceholderUserName")]
        public string UserName { get; set; }

        [DisplayName("ユーザーカナ")]
        [TextBoxDataSource(PlaceHolder = "ユーザーカナ")]
        public string UserKana { get; set; }

        [DisplayName("クラス")]
        [ClassDataSource()]
        public int ClassIndex { get; set; }

        [DisplayName("削除タップ")]
        [DeleteDataSource()]
        public int? DelFlag { get; set; }

        [DisplayName("表示")]
        [YesNoDataSource(YesName="はい",NoName="いいえ")]
        public int? DisFlag { get; set; }

        [DisplayName("我能")]
        [CheckBoxDataSourceAttribute(ViewDataKey = "DeleteListItems")]
        public List<string> CanDoList { get; set; }
        [TextAreaDataSource]
        public string MultiText { get; set; }

        [ScaffoldColumn(false)]
        public DateTime LastUpdateTime { get; set; }
    }

    [ListViewModel(Title = "会员卡", SubTitle = "会员卡管理")]
    [EditViewModel(Title="会员卡",SubTitle="会员卡管理")]
    public class MemberCardViewModel 
    {
        [HiddenInput(DisplayValue=false)]
        public string MemberCardId { get; set; }
        [Display(Name = "用户ID")]
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "组织ID")]
        public string OrganizationId { get; set; }
        [Display(Name = "卡号")]
        public string CardNo { get; set; }

    }


    /*
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

    */
    
    class ClassDataSourceAttribute : DropDownDataSourceAttributeBase
    {
        public ClassDataSourceAttribute()
        { 
            for (int i = 0; i < 10; i++)
            {
                this.ListItems.Add(new SelectListItem() { Value = i.ToString(), Text = "Class" + i });
            }
        }
    }

    class DeleteDataSourceAttribute : RadioButtonDataSourceAttributeBase
    {
        public DeleteDataSourceAttribute()
        { 
            this.ListItems.Add(new SelectListItem() { Text = "正常使用", Value = "1" });
            this.ListItems.Add(new SelectListItem() { Text = "逻辑删除", Value = "2" });
            this.ListItems.Add(new SelectListItem() { Text = "物理删除", Value = "3" });        
        }
    }

}
