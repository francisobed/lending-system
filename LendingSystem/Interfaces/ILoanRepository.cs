using LendingSystem.Entities;
using LendingSystem.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LendingSystem.Interfaces
{
    public interface ILoanRepository
    {
        Task AddAsync(Loan loan);
        Task<IEnumerable<Loan>> GetAllLoansAsync();
        Task<IEnumerable<Repayment>> GetAllRepaymentsAsync();
        Task<Loan?> GetByIdAsync(Guid id);
        Task<Loan?> GetByIdWithScheduleAsync(Guid loanId);
        Task<IEnumerable<Loan>> GetByUserIdAsync(Guid userId);
        Task<Loan?> GetByUserIdWithScheduleAsync(Guid userId);
        Task<LoanSummaryDto> GetLoanSummaryAsync();
        Task SaveChangesAsync();
    }
}
