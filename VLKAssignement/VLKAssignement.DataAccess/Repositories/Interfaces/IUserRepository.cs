using System;

namespace VLKAssignement.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository:IRepositoryBase<Models.User>    
    {
        Guid GetCartId(Guid userId);
    }
}
