using System;
using System.Collections.Generic;
using VLKAssignement.DataAccess.Models;

namespace VLKAssignement.Service.Interfaces
{
    public interface ITransferCartService
    {
        List<Transfer> GetTransfersByUserAndStatus(Guid userId, string status);
    }
}
