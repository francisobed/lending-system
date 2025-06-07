using LendingSystem.Entities;
using System.Threading.Tasks;

namespace LendingSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddUserAsync(User user);
        // More methods as needed
    }
}
