using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Noregold.Entities.Interface
{
    public interface IAppDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }

        Task<int> ExecuteSqlRawAsync(string storedProcedure, List<SqlParameter> parameters);
        Task<int> ExecuteScalarAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, DynamicParameters? parameters, CommandType commandType);
        Task<T?> QuerySingleOrDefaultAsync<T>(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
    }
}
