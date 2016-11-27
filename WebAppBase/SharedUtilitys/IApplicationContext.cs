using Pasys.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pasys.Core
{
    public interface IApplicationContext
    {
        //IUser CurrentUser { get; }
        string VirtualPath { get; }
    }
}
