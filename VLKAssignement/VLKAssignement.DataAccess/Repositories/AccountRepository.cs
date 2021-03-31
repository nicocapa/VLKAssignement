using VLKAssignement.DataAccess.Repositories.Interfaces;

namespace VLKAssignement.DataAccess.Repositories
{
    public class AccountRepository:RepositoryBase<Models.Account>,IAccountRepository    
    {
        public AccountRepository(AppDbContext context) : base(context) { }
    }
}
