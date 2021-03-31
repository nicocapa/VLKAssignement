using Microsoft.EntityFrameworkCore;
using VLKAssignement.DataAccess.Models;

namespace VLKAssignement.DataAccess
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {            
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<CachedExchangeRate> CachedExchangeRates { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<TransferCart> TransfersCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransferCart>()
                .HasMany(c => c.Transfers)
                .WithOne(e => e.Cart)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            var oneUser = new User { Name = "Robert", LastName = "Zemeckis" };
            var oneAccount = new Account { UserId = oneUser.Id, IBAN = "NL25INGB1234567899", CurrencyCode = "EUR", Balance = 1000000 };
            modelBuilder.Entity<User>().HasData(oneUser);
            modelBuilder.Entity<Account>().HasData(oneAccount);

        }        
    }
}
