using System;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using VLKAssignement.Service.Interfaces;
using System.Linq;

namespace VLKAssignement.Service
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepository;        
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public DataAccess.Models.Account GetByUserId(Guid userId)
        {
            var account = _accountRepository.FindAll(a => a.UserId == userId).FirstOrDefault();
            if(account == null)
            {
                throw new ArgumentException("The specified user does not have an account.");
            }
            return account;
        }
    }
}
