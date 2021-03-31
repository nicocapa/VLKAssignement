using System;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.DataAccess.Repositories.Interfaces;

namespace VLKAssignement.DataAccess.Repositories
{
    public class TransferRepository : RepositoryBase<Models.Transfer>, ITransferRepository    
    {
        public TransferRepository(AppDbContext context) : base(context) { }

        public Guid SignTransfer(Transfer transfer, Transaction transaction, Account account)
        {
            _context.Transfers.Update(transfer);
            _context.Accounts.Update(account);
            var result = _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public override Transfer Update(Transfer item)
        {
            item.UpdatedOn = DateTime.UtcNow;
            return base.Update(item);
        }
    }
}
