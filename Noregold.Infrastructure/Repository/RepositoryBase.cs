using Dapper;
using Noregold.Entities.Interface;
using Noregold.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Noregold.Infrastructure.Repository
{
    public abstract class RepositoryBase<T>(IAppDbContext context) : IRepositoryBase<T> where T : class
    {
        protected IAppDbContext _context = context;

        public virtual async Task<int> ExecuteScalarAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true) => await _context.ExecuteScalarAsync(storedProcedure, parameters, isStoredProcedure);


        public virtual async Task<IEnumerable<T1>> QueryAsync<T1>(string storedProcedure, DynamicParameters? parameters, CommandType commandType) => await _context.QueryAsync<T1>(storedProcedure, parameters, commandType);


        public virtual async Task<IEnumerable<T>> QueryAsync(string storedProcedure, DynamicParameters? parameters, CommandType commandType) => await _context.QueryAsync<T>(storedProcedure, parameters, commandType);


        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true) => await _context.QueryMultipleAsync(storedProcedure, parameters, isStoredProcedure);

        // public async Task<T> QuerySingleAsync(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true) => await _context.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, isStoredProcedure);

        public async Task<T1?> QuerySingleOrDefaultAsync<T1>(string storedProcedure, DynamicParameters? parameters, bool isStoredProcedure = true) => await _context.QuerySingleOrDefaultAsync<T1>(storedProcedure, parameters, isStoredProcedure);

    }
}
