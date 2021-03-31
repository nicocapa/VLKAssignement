using System;
using System.Collections.Generic;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using VLKAssignement.Service.Interfaces;
using System.Linq;

namespace VLKAssignement.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;        

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;            
        }

        public List<DataAccess.Models.Transaction> GetByUserId(Guid userId)
        {
            return _transactionRepository.FindAll(a => a.UserId == userId).ToList();            
        }
    }
}
