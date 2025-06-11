using LendingSystem.Entities;
using LendingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LendingSystem.API.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/loans")]
    public class AdminLoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;

        public AdminLoanController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _loanRepository.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveLoan(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null) return NotFound();

            loan.Status = LoanStatus.Approved;
            await _loanRepository.SaveChangesAsync();

            return Ok(new { message = "Loan approved" });
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectLoan(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null) return NotFound();

            loan.Status = LoanStatus.Rejected;
            await _loanRepository.SaveChangesAsync();

            return Ok(new { message = "Loan rejected" });
        }

        [HttpPost("{id}/disburse")]
        public async Task<IActionResult> DisburseLoan(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null) return NotFound();

            if (loan.Status != LoanStatus.Approved)
                return BadRequest("Loan must be approved before disbursement.");

            loan.Status = LoanStatus.Disbursed;
            loan.DisbursedAt = DateTime.UtcNow;

            await _loanRepository.SaveChangesAsync();
            return Ok(new { message = "Loan disbursed successfully." });
        }

        [HttpGet("repayments")]
        public async Task<IActionResult> GetAllRepayments()
        {
            var repayments = await _loanRepository.GetAllRepaymentsAsync();
            return Ok(repayments);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetLoanStatistics()
        {
            var stats = await _loanRepository.GetLoanSummaryAsync();
            return Ok(stats);
        }

        // New Schedule endpoints for Admin

        // GET api/admin/loans/{loanId}/schedule
        [HttpGet("{loanId}/schedule")]
        public async Task<IActionResult> GetLoanRepaymentSchedule(Guid loanId)
        {
            var loan = await _loanRepository.GetByIdWithScheduleAsync(loanId);
            if (loan == null) return NotFound();

            return Ok(loan.RepaymentSchedule.Select(r => new
            {
                r.Id,
                r.DueDate,
                r.Amount,
                r.IsPaid,
                r.PaidDate
            }));
        }

        // POST api/admin/loans/{loanId}/schedule/generate
        [HttpPost("{loanId}/schedule/generate")]
        public async Task<IActionResult> GenerateRepaymentSchedule(Guid loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return NotFound();

            // Assume you have a method to generate schedule
            loan.InitializeRepayment(10); // Example: 10% interest or dynamic value
            await _loanRepository.SaveChangesAsync();

            return Ok(new { message = "Repayment schedule generated successfully." });
        }
    }
}
