using LendingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LendingSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetByUsernameOrEmailAsync(string username, string email);
    }
}
