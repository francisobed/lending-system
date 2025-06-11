namespace LendingSystem.API.DTOs.User
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User"; // Default to User if not provided
    }
}
