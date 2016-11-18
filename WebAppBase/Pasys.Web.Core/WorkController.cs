using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pasys.Web.Core
{
    public interface IWorkContext 
    { 
    
    }

    public interface IWorkController<T> where T : IWorkContext
    {
        T WorkContext { get; }
    }
}
