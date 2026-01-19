using Dapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Noregold.Entities.Interface;
using Noregold.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Noregold.Entities
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options), IAppDbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();

        public async Task<int> ExecuteScalarAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            return await Connection.ExecuteScalarAsync<int>(storedProcedure, parameters, null, 90, commandType);
        }

        public async Task<int> ExecuteSqlRawAsync(string storedProcedure, List<SqlParameter> parameters)
        {
            return await Database.ExecuteSqlRawAsync(storedProcedure, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, DynamicParameters? parameters, CommandType commandType)
        {
            return await Connection.QueryAsync<T>(storedProcedure, parameters, null, 90, commandType);
        }


        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            return await Connection.QueryMultipleAsync(storedProcedure, parameters, null, 90, commandType);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true)
        {
            var commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            return await Connection.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, null, 90, commandType);
        }
    }
}
