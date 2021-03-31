using System;
using System.Collections.Generic;

namespace VLKAssignement.Service.Interfaces
{
    public interface ITransactionService
    {
        List<DataAccess.Models.Transaction> GetByUserId(Guid userId);
    }
}