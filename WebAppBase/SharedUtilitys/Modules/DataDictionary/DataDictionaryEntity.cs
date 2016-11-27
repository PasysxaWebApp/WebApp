﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pasys.Core.MetaData;
using Pasys.Core.Models;

namespace Pasys.Core.Modules.DataDictionary
{
    [DataConfigure(typeof(DataDictionaryEntityMetaData))]
    public class DataDictionaryEntity : IImage
    {
        public long ID { get; set; }
        public string DicName { get; set; }

        public string DicValue { get; set; }

        public int Order { get; set; }

        public long Pid { get; set; }

        public bool IsSystem { get; set; }

        public string ImageUrl { get; set; }

        public string ImageThumbUrl { get; set; }


        public string Title { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatebyName { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public string LastUpdateBy { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdateByName { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }


    }
    class DataDictionaryEntityMetaData : DataViewMetaData<DataDictionaryEntity>
    {
        protected override void DataConfigure()
        {
        }

        protected override void ViewConfigure()
        {
            ViewConfig(m => m.ID).AsHidden();
            ViewConfig(m => m.DicName).AsTextBox().Required().MaxLength(25);
            ViewConfig(m => m.IsSystem).AsCheckBox().ReadOnly();
            ViewConfig(m => m.DicValue).AsTextBox();
            ViewConfig(m => m.Title).AsHidden();
            ViewConfig(m => m.Pid).AsHidden();
        }
    }

}
