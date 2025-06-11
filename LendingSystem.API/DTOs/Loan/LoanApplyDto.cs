namespace LendingSystem.API.DTOs.Loan
{
    public class LoanApplyDto
    {
        public decimal Amount { get; set; }
        public int TermMonths { get; set; }
    }
}
