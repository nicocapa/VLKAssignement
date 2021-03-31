using System;
using System.Collections.Generic;
using VLKAssignement.DataAccess.Models;

namespace VLKAssignement.Service.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
    }
}
