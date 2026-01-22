using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Noregold.Entities.Models;
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

        public DbSet<Inventory> Inventories { get; set; }

        Task<int> ExecuteSqlRawAsync(string storedProcedure, List<SqlParameter> parameters);
        Task<int> ExecuteScalarAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<IReadOnlyList<T>> QueryAsync<T>(string storedProcedure, DynamicParameters? parameters, CommandType commandType);
        Task<T?> QuerySingleOrDefaultAsync<T>(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true);
    }
}
