﻿using Pasys.Core.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pasys.Core.Extend;
using System.Collections;
using Pasys.Core.ViewPort.Validator;

namespace Pasys.Core.ViewPort.Descriptor
{
    public class ListEditorDescriptor : BaseDescriptor
    {
        public ListEditorDescriptor(Type modelType, string property)
            : base(modelType, property)
        {

            this.TagType = HTMLEnumerate.HTMLTagTypes.ListEditor;
            this.TemplateName = "ListEditor";
        }
        #region Regular
        public ListEditorDescriptor SetDisplayName(string name)
        {
            this.DisplayName = name;
            foreach (ValidatorBase item in this.Validator)
            {
                item.DisplayName = name;
            }
            return this;
        }


        public ListEditorDescriptor AddStyle(string properyt, string value)
        {
            if (this.Styles.ContainsKey(properyt))
            {
                this.Styles[properyt] = value;
            }
            else
            {
                this.Styles.Add(properyt, value);
            }
            return this;
        }
        public ListEditorDescriptor Hide()
        {
            this.IsHidden = true;
            return this;
        }
        public ListEditorDescriptor Ignore()
        {
            this.IsIgnore = true;
            return this;
        }

        public ListEditorDescriptor Order(int index)
        {
            this.OrderIndex = index;
            return this;
        }
        public ListEditorDescriptor ShowForDisplay(bool show)
        {
            this.IsShowForDisplay = show;
            return this;
        }
        public ListEditorDescriptor ShowForEdit(bool show)
        {
            this.IsShowForEdit = show;
            return this;
        }
        public ListEditorDescriptor SetTemplate(string template)
        {
            this.TemplateName = template;
            return this;
        }

        #endregion

    }
}
