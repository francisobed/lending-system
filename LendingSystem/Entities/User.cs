using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LendingSystem.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // e.g. "User", "Admin"

        public List<Loan> Loans { get; set; } = new();

        [NotMapped]
        public string Username { get; set; } // Not stored in DB
    }
}
