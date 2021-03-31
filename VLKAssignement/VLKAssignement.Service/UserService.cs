using VLKAssignement.DataAccess.Repositories.Interfaces;
using VLKAssignement.Service.Interfaces;
using System.Collections.Generic;

namespace VLKAssignement.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;        
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<DataAccess.Models.User> GetAll()
        {
            return _userRepository.FindAll();
        }
    }
}
