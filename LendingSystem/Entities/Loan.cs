using LendingSystem.Entities;

public class Loan
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public decimal Amount { get; set; }
    public int TermMonths { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }
    public Guid? ReviewedBy { get; set; }
    public User? Reviewer { get; set; }
    public DateTime? DisbursedAt { get; set; }
    public decimal Balance { get; set; }
    public decimal MonthlyRepayment { get; set; }

    // ADD THIS:
    public ICollection<RepaymentSchedule> RepaymentSchedule { get; set; } = new List<RepaymentSchedule>();

    public void InitializeRepayment(decimal interestRatePercent)
    {
        var totalRepayable = Amount * (1 + interestRatePercent / 100);
        MonthlyRepayment = Math.Round(totalRepayable / TermMonths, 2);
        Balance = totalRepayable;
    }
}
