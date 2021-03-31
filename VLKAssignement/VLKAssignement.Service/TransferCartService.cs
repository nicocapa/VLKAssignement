using System;
using System.Collections.Generic;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using VLKAssignement.DataAccess.Models;

using VLKAssignement.Service.Interfaces;
using System.Linq;

namespace VLKAssignement.Service
{
    public class TransferCartService : ITransferCartService
    {
        private readonly ITransferCartRepository _transferCartRepository;       

        public TransferCartService(ITransferCartRepository transferCartRepository)
        {
            _transferCartRepository = transferCartRepository;            
        }

        public List<DataAccess.Models.Transfer> GetTransfersByUserAndStatus(Guid userId, string status)
        {                        
            var cart = _transferCartRepository.GetByUser(userId);
            if (cart == null)
            {
                return new List<Transfer>();
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                return cart.Transfers.ToList();
            }
            else
            {
                var filteredTransfers = cart.Transfers.Where(c => c.Status.ToLower() == status.ToLower());
                return filteredTransfers.ToList();
            }            
        }
    }
}
