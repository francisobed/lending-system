using LendingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations if any
            base.OnModelCreating(modelBuilder);
        }
    }
}
