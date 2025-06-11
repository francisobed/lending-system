// File: Entities/Repayment.cs
using System;

namespace LendingSystem.Entities
{
    public class Repayment
    {
        public Guid Id { get; set; }
        public Guid LoanId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
        public string? PaymentMethod { get; set; }

        // Navigation property
        public Loan? Loan { get; set; }
    }
}
