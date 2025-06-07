using LendingSystem.Entities;
using LendingSystem.Interfaces;
using LendingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LendingSystem.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLoanAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }

        public async Task<Loan> GetByIdAsync(Guid loanId)
        {
            return await _context.Loans.FindAsync(loanId);
        }

        public async Task<IEnumerable<Loan>> GetLoansByUserIdAsync(Guid userId)
        {
            return await _context.Loans
                .AsNoTracking()
                .Where(l => l.BorrowerId == userId)
                .ToListAsync();
        }

        public async Task UpdateLoanAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }
    }
}
