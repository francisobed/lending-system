namespace LendingSystem.API.DTOs.Admin
{
    public class AdminLoanActionResultDto
    {
        public string Message { get; set; } = null!;
        public Guid LoanId { get; set; }
        public string NewStatus { get; set; } = null!;
    }
}
