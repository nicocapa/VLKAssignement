using System;
using VLKAssignement.DataAccess.Models;
using System.Linq;

namespace VLKAssignement.DataAccess.Repositories.Interfaces
{
    public class TransferCartRepository : RepositoryBase<TransferCart>, ITransferCartRepository
    {
        public TransferCartRepository(AppDbContext context) : base(context) { }

        public TransferCart GetByUser(Guid userId)
        {
            return _context.TransfersCart.FirstOrDefault(c => c.UserId == userId);
        }
    }
}
