using VLKAssignement.DataAccess.Repositories.Interfaces;

namespace VLKAssignement.DataAccess.Repositories
{
    public class TransactionRepository : RepositoryBase<Models.Transaction>, ITransactionRepository    
    {
        public TransactionRepository(AppDbContext context):base(context) { }
    }
}
