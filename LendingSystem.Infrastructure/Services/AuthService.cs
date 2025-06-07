using LendingSystem.Entities;
using LendingSystem.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LendingSystem.Infrastructure.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(string email, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null) throw new Exception("User already exists");

            var user = new User
            {
                Email = email,
                PasswordHash = HashPassword(password),
                Role = "User"
            };

            await _userRepository.AddUserAsync(user);
            return user;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            var hash = HashPassword(password);
            if (user.PasswordHash != hash) return null;

            return user;
        }
    }
}
