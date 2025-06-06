using LendingSystem;
using Microsoft.EntityFrameworkCore;

namespace LendingSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Loan> Loans { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations
            base.OnModelCreating(modelBuilder);
        }
    }
}
