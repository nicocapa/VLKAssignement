using System;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using System.Linq;

namespace VLKAssignement.DataAccess.Repositories
{
    public class UserRepository:RepositoryBase<Models.User>, IUserRepository    
    {
        public UserRepository(AppDbContext context):base(context) { }

        public Guid GetCartId(Guid userId)
        {
            var cart = _context.TransfersCart.FirstOrDefault(c => c.UserId == userId);
            if(cart == null)
            {
                cart = new Models.TransferCart
                {
                    UserId = userId
                };
                var result = _context.TransfersCart.Add(cart);
                cart = result.Entity;
            }
            return cart.Id;
        }
    }
}
