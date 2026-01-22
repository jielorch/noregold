using Noregold.Entities.Interface;
using Noregold.Infrastructure.Interface;
using Noregold.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noregold.Infrastructure
{
    public class UnitOfWork(IAppDbContext context) : IUnitOfWork, IDisposable
    {
        private readonly IAppDbContext _context = context;

        private IInventoryRepository? _inventoryRepository;

        public IInventoryRepository InventoryRepository
        {
            get
            {
                _inventoryRepository ??= new InventoryRepository(_context);
                return _inventoryRepository;
            }
        }

        public void Dispose()
        {
            if (_context.Connection.State == System.Data.ConnectionState.Open)
            {
                _context.Connection.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
