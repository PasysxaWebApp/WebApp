﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pasys.Web.Core
{
    public class AjaxResult
    {
        public AjaxResult()
        {

        }
        public AjaxResult(AjaxStatus status, string msg)
        {
            this.Message = msg;
            this.Status = status;
        }
        public string Message { get; set; }
        public AjaxStatus Status { get; set; }
    }
    public enum AjaxStatus
    {
        Normal = 1,
        Warn = 2,
        Error = 3
    }
}
