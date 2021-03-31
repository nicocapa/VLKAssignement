using System;
using System.Threading.Tasks;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.Domain;

namespace VLKAssignement.Service.Interfaces
{
    public interface ITransferService
    {
        Guid Add(Transfer transfer);
        Task<SignResult> Sign(Guid transferId);
    }
}