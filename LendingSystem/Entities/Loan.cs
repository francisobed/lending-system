namespace LendingSystem.Entities
{
    public enum LoanStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Loan
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public decimal Amount { get; set; }
        public int TermMonths { get; set; }  // e.g., 3 months
        public LoanStatus Status { get; set; } = LoanStatus.Pending;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        // Simple repayment schedule: amount + interest / term
        public decimal MonthlyRepayment => Math.Round(Amount * 1.1m / TermMonths, 2); // 10% interest
    }
}
