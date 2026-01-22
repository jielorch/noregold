using Noregold.Entities.Interface;
using Noregold.Entities.Models;
using Noregold.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noregold.Infrastructure.Repository
{
    public class InventoryRepository(IAppDbContext context): RepositoryBase<Inventory>(context), IInventoryRepository
    {
    }
}
