using LendingSystem.Entities;

namespace LendingSystem.Interfaces
{
    public interface ILoanRepository
    {
        Task AddAsync(Loan loan);
        Task<Loan?> GetByIdAsync(Guid id);
        Task<IEnumerable<Loan>> GetByUserIdAsync(Guid userId);
        Task SaveChangesAsync();
    }
}
