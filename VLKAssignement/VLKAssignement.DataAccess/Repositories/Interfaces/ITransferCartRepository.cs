using System;
using VLKAssignement.DataAccess.Models;
using AutoMapper;

namespace VLKAssignement.DataAccess.Repositories.Interfaces
{
    public interface ITransferCartRepository:IRepositoryBase<Models.TransferCart>    
    {
        TransferCart GetByUser(Guid userId);
    }
}
