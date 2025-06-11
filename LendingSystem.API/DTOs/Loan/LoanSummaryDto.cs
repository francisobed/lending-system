namespace LendingSystem.API.DTOs.Loan
{
    public class LoanSummaryDto
    {
        public int TotalLoans { get; set; }
        public int ApprovedLoans { get; set; }
        public int RejectedLoans { get; set; }
        public decimal TotalLoanAmount { get; set; }
    }
}
