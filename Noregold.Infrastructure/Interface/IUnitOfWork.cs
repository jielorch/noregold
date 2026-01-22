using System;
using System.Collections.Generic;
using System.Text;

namespace Noregold.Infrastructure.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IInventoryRepository InventoryRepository { get; }
    }
}
