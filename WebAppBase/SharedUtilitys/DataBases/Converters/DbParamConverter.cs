using System.Data;
using Pasys.Core.DataBases.Base;
using Pasys.Core.DataBases.Configs;
using Pasys.Core.DataBases.Converters.Extractors;

namespace Pasys.Core.DataBases.Converters
{
    public static class DbParamConverter
    {
        public static IDbDataParameter Convert(SqlParamWrapper parameter)
        {
            switch (DatabaseConfig.DatabaseType)
            {
                case DatabaseEnum.SqlServer:
                    return SqlServerExtractor.Extract(parameter);  

                case DatabaseEnum.MySql:
                    return MySqlExtractor.Extract(parameter);  
            }

            return null;
        }
    }
}
