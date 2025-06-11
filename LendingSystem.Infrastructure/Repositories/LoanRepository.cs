using LendingSystem.Entities;
using LendingSystem.Interfaces;
using LendingSystem.Infrastructure.Data;
using LendingSystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        public async Task<IEnumerable<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans.ToListAsync();
        }

        public async Task<IEnumerable<Repayment>> GetAllRepaymentsAsync()
        {
            return await _context.Repayments.ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(Guid id)
        {
            return await _context.Loans.FindAsync(id);
        }

        public async Task<Loan?> GetByIdWithScheduleAsync(Guid loanId)
        {
            return await _context.Loans
                .Include(l => l.RepaymentSchedule)
                .FirstOrDefaultAsync(l => l.Id == loanId);
        }

        public async Task<IEnumerable<Loan>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Loans
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<Loan?> GetByUserIdWithScheduleAsync(Guid userId)
        {
            return await _context.Loans
                .Include(l => l.RepaymentSchedule)
                .Where(l => l.UserId == userId &&
                            (l.Status == LoanStatus.Disbursed || l.Status == LoanStatus.Approved))
                .FirstOrDefaultAsync();
        }

        public async Task<LoanSummaryDto> GetLoanSummaryAsync()
        {
            var totalLoans = await _context.Loans.CountAsync();
            var approvedLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Approved);
            var rejectedLoans = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Rejected);
            var totalLoanAmount = await _context.Loans.SumAsync(l => l.Amount);

            return new LoanSummaryDto
            {
                TotalLoans = totalLoans,
                ApprovedLoans = approvedLoans,
                RejectedLoans = rejectedLoans,
                TotalLoanAmount = totalLoanAmount
            };
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
