using LendingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<RepaymentSchedule> RepaymentSchedules => Set<RepaymentSchedule>();

        // Fix: Change 'Repayments' from 'object' to DbSet<Repayment> to allow EF Core querying
        public DbSet<Repayment> Repayments => Set<Repayment>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Loan>()
                .HasMany(l => l.RepaymentSchedule)
                .WithOne(rs => rs.Loan)
                .HasForeignKey(rs => rs.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
