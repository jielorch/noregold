using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noregold.Service.Interrface
{
    public interface IInventoryService
    {
        Task<IReadOnlyList<T>> GetInventoryDetailsAsync<T>(string command, DynamicParameters parameters);
        Task<int> BulkUploadAsync(string command, DynamicParameters parameters);
    }
}
