using System;

namespace VLKAssignement.DataAccess.Repositories.Interfaces
{
    public interface ITransferRepository : IRepositoryBase<Models.Transfer>    
    {
        Guid SignTransfer(Models.Transfer transfer, Models.Transaction transaction, Models.Account account);
    }
}