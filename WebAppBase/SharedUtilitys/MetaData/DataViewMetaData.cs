﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Pasys.Core.ViewPort;
using Pasys.Core.ViewPort.Descriptor;
using Pasys.Core.Data;
using Pasys.Core.Constant;
using Pasys.Core.Models;
using System.ComponentModel;
using System.Reflection;
using Pasys.Core;
using Pasys.Core.Extend;
using Microsoft.Practices.ServiceLocation;

namespace Pasys.Core.MetaData
{
    /// <summary>
    /// 数据元数据特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataViewMetaData<T> : IDataViewMetaData where T : class
    {
        public DataViewMetaData()
        {
            Init();
        }
        public void Init()
        {
            ViewPortDescriptors = new Dictionary<string, BaseDescriptor>();
            TargetType = typeof(T);
            foreach (var item in TargetType.GetProperties())
            {
                TypeCode code = Type.GetTypeCode(item.PropertyType.IsGenericType ? item.PropertyType.GetGenericArguments()[0] : item.PropertyType);
                switch (code)
                {
                    case TypeCode.Boolean:
                        ViewConfig(item.Name).AsCheckBox().SetColumnWidth(75);
                        break;
                    case TypeCode.Char:
                        ViewConfig(item.Name).AsTextBox().MaxLength(1).RegularExpression(RegularExpression.Letters);
                        break;
                    case TypeCode.DateTime:
                        ViewConfig(item.Name).AsTextBox().FormatAsDate().SetColumnWidth(140);
                        break;
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        ViewConfig(item.Name).AsTextBox().RegularExpression(RegularExpression.PositiveIntegersAndZero).SetColumnWidth(75);
                        break;
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        ViewConfig(item.Name).AsTextBox().RegularExpression(RegularExpression.Integer).SetColumnWidth(75);
                        break;
                    case TypeCode.Object:
                        ViewConfig(item.Name).AsHidden().Ignore();
                        break;
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        ViewConfig(item.Name).AsTextBox().RegularExpression(RegularExpression.Float).SetColumnWidth(75);
                        break;
                    case TypeCode.String:
                        ViewConfig(item.Name).AsTextBox().MaxLength(200).SetColumnWidth(200);
                        break;
                    case TypeCode.DBNull:
                    case TypeCode.Byte:
                    case TypeCode.Empty:
                    default: ViewConfig(item.Name).AsTextBox();
                        break;
                }
            }
            if (typeof(EditorEntity).IsAssignableFrom(TargetType))
            {
                ViewConfig("CreateBy").AsHidden();
                ViewConfig("CreatebyName").AsTextBox().Hide().SetColumnWidth(80);
                ViewConfig("CreateDate").AsTextBox().Hide().FormatAsDateTime().SetColumnWidth(140);

                ViewConfig("LastUpdateBy").AsHidden();
                ViewConfig("LastUpdateByName").AsTextBox().Hide().SetColumnWidth(80);
                ViewConfig("LastUpdateDate").AsTextBox().Hide().FormatAsDateTime().SetColumnWidth(140);
                ViewConfig("ActionType").AsHidden().AddClass("ActionType");
                ViewConfig("Title").AsTextBox().Order(1).SetColumnWidth(200);
                ViewConfig("Description").AsTextArea().SetColumnWidth(250).Order(101);
                ViewConfig("Status").AsDropDownList().DataSource(DicKeys.RecordStatus, SourceType.Dictionary).SetColumnWidth(70);

            }
            if (typeof(IImage).IsAssignableFrom(TargetType))
            {
                ViewConfig("ImageUrl").AsTextBox().HideInGrid();
                ViewConfig("ImageThumbUrl").AsTextBox().HideInGrid();
            }
            if (IsIgnoreBase())
            {
                IgnoreBase();
            }
            DataConfigure();
            ViewConfigure();
        }

        public Dictionary<string, BaseDescriptor> ViewPortDescriptors
        {
            get;
            set;
        }

        public Dictionary<string, PropertyDataInfo> PropertyDataConfig
        {
            get;
            set;
        }

        public Type TargetType
        {
            get;
            private set;
        }

        public Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                var properties = new Dictionary<string, PropertyInfo>();
                TargetType.GetProperties().Each(m => properties.Add(m.Name, m));
                return properties;
            }
        }
        /// <summary>
        /// 数据配置 方法[DataConfig][DataTable][DataPrimaryKey]
        /// </summary>
        protected abstract void DataConfigure();
        /// <summary>
        /// 视图配置 方法[ViewConfig]
        /// </summary>
        protected abstract void ViewConfigure();
        public virtual DataFilter DataAccess(DataFilter filter)
        {
            return filter;
        }

        /// <summary>
        /// 视图配置，界面显示
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected TagsHelper ViewConfig(Expression<Func<T, object>> expression)
        {
            string key = Reflection.LinqExpression.GetPropertyName(expression.Body);
            return ViewConfig(key);
        }
        /// <summary>
        /// 视图配置，界面显示
        /// </summary>
        /// <param name="properyt">实体字段名称</param>
        /// <returns></returns>
        protected TagsHelper ViewConfig(string properyt)
        {
            return new TagsHelper(properyt, ViewPortDescriptors, TargetType, TargetType.GetProperty(properyt));
        }
        /// <summary>
        /// 主键
        /// </summary>
        /// <summary>
        ///  数据配置，与数据库对应
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected PropertyDataInfoHelper<T> DataConfig(Expression<Func<T, object>> expression)
        {
            string key = Reflection.LinqExpression.GetPropertyName(expression.Body);
            return DataConfig(key);
        }
        /// <summary>
        /// 数据配置，与数据库对应
        /// </summary>
        /// <param name="property">实体字段名称</param>
        /// <returns></returns>
        protected PropertyDataInfoHelper<T> DataConfig(string property)
        {
            PropertyDataInfo data;
            if (PropertyDataConfig.ContainsKey(property))
                data = PropertyDataConfig[property];
            else
            {
                data = new PropertyDataInfo(property);
                data.TableAlias = "T0";
                data.ColumnName = property;
                PropertyDataConfig.Add(property, data);
            }
            data.Ignore = false;
            return new PropertyDataInfoHelper<T>(data, this);
        }

        /// <summary>
        /// 将属于基类的字段全部设为DataConfig(item.Name).Ignore();
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsIgnoreBase()
        {
            return false;
        }
        private void IgnoreBase()
        {
            PropertyDescriptorCollection attrs = TypeDescriptor.GetProperties(this.TargetType.BaseType);
            foreach (PropertyDescriptor item in attrs)
            {
                ViewConfig(item.Name).AsHidden();
            }
        }
    }

}
