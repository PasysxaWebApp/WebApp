﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pasys.Core.Extend;

namespace Pasys.Core.ViewPort.Validator
{
    public class RequiredValidator : ValidatorBase
    {
        public RequiredValidator()
        {
            this.BaseErrorMessage = "请输入{0}";
        }
        public override bool Validate(object value)
        {
            if (value == null || value.ToString().IsNullOrEmpty()) return false;
            else return true;
        }
    }
}
