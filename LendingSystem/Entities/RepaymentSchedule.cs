namespace LendingSystem.Entities
{
    public class RepaymentSchedule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LoanId { get; set; }
        public Loan Loan { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } = false;

       // public ICollection<RepaymentSchedule> RepaymentSchedule { get; set; }

    }
}
