using LendingSystem.Entities;
using LendingSystem.Interfaces;
using LendingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LendingSystem.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;
        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        public async Task<Loan?> GetByIdAsync(Guid id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task<IEnumerable<Loan>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Loans.Where(l => l.UserId == userId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
