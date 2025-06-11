namespace LendingSystem.API.DTOs.Loan
{
    public class RepaymentScheduleDto
    {
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public bool IsPaid { get; set; }
    }
}
