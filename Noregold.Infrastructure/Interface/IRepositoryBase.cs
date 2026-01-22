using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Noregold.Infrastructure.Interface
{
    public interface IRepositoryBase<T>
    {
        Task<int> ExecuteScalarAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<IEnumerable<T1>> QueryAsync<T1>(string storedProcedure, DynamicParameters? parameters, CommandType commandType);
        Task<IEnumerable<T>> QueryAsync(string storedProcedure, DynamicParameters? parameters, CommandType commandType);
        // Task<T> QuerySingleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<T1?> QuerySingleOrDefaultAsync<T1>(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
    }
}
