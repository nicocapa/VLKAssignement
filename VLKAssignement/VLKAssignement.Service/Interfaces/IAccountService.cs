using System;
using VLKAssignement.DataAccess.Models;

namespace VLKAssignement.Service.Interfaces
{
    public interface IAccountService
    {
        Account GetByUserId(Guid userId);
    }
}
