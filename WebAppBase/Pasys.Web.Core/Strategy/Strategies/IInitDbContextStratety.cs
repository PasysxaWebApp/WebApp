using SharedUtilitys.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Core
{
    public interface IInitDbContextStratety : IStrategy
    {
        void InitDbContext();
    }
}
