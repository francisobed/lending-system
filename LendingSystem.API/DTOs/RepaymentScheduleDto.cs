public class RepaymentScheduleDto
{
    public DateTime DueDate { get; set; }
    public decimal AmountDue { get; set; }
    public bool IsPaid { get; set; }  // optional flag if you implement repayment records
}
