namespace LendingSystem.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // e.g. "User", "Admin"
        public List<Loan> Loans { get; set; } = new();
    }
}
