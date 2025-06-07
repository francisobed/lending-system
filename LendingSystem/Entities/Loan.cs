namespace LendingSystem.Entities
{
    public class Loan
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime AppliedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public User Borrower { get; set; }
        public Guid BorrowerId { get; set; }
    }
}
