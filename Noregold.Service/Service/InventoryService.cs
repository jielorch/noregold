using Dapper;
using Noregold.Infrastructure.Interface;
using Noregold.Service.Interrface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noregold.Service.Service
{
    public class InventoryService(IUnitOfWork unitOfWork) : IInventoryService
    {
        public async Task<IReadOnlyList<T>> GetInventoryDetailsAsync<T>(string command, DynamicParameters parameters)
        {
            var result = await unitOfWork.InventoryRepository.QueryAsync<T>(command, parameters, System.Data.CommandType.Text);
            return [.. result];
        }
    }
}
