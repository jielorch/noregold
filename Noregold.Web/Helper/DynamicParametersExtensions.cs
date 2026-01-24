using Dapper;
using System.Data;

namespace Noregold.Web.Helper
{
    public static class DynamicParametersExtensions
    {
        public static DynamicParameters AddTableValuedParameter(
              this DynamicParameters parameters,
                      string parameterName,
                      DataTable table,
                      string typeName)
        {
            parameters.Add(parameterName, table.AsTableValuedParameter(typeName));
            return parameters;
        }

        public static DynamicParameters AddOutputParameter(
            this DynamicParameters parameters,
            string parameterName,
            DbType dbType)
        {
            parameters.Add(parameterName, dbType: dbType, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
