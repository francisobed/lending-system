using LendingSystem.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LendingSystem.Interfaces
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetLoansByUserIdAsync(Guid userId);
        Task AddLoanAsync(Loan loan);
        Task<Loan> GetByIdAsync(Guid loanId);
        Task UpdateLoanAsync(Loan loan);
    }
}
