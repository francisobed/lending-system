using LendingSystem.API.DTOs;
using LendingSystem.Entities;
using LendingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;  // Needed for Select and FirstOrDefault
using System;
using System.Threading.Tasks;

namespace LendingSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUserRepository _userRepository;

        public LoanController(ILoanRepository loanRepository, IUserRepository userRepository)
        {
            _loanRepository = loanRepository;
            _userRepository = userRepository;
        }

        // User applies for loan
        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] LoanApplyDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                UserId = parsedUserId,
                Amount = dto.Amount,
                TermMonths = dto.TermMonths,
                Status = LoanStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

            // Assuming 10% interest
            loan.InitializeRepayment(10);

            await _loanRepository.AddAsync(loan);
            await _loanRepository.SaveChangesAsync();

            return Ok(new
            {
                loan.Id,
                loan.Amount,
                loan.TermMonths,
                loan.Status,
                loan.MonthlyRepayment
            });
        }

        // User views their loans
        [HttpGet("myloans")]
        public async Task<IActionResult> GetMyLoans()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var loans = await _loanRepository.GetByUserIdAsync(parsedUserId);
            return Ok(loans.Select(l => new
            {
                l.Id,
                l.Amount,
                l.TermMonths,
                l.Status,
                l.MonthlyRepayment,
                l.AppliedAt
            }));
        }

        // Admin approves loan
        [Authorize(Roles = "Admin")]
        [HttpPost("{loanId}/approve")]
        public async Task<IActionResult> ApproveLoan(Guid loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return NotFound();

            loan.Status = LoanStatus.Approved;
            await _loanRepository.SaveChangesAsync();
            return Ok(new { message = "Loan approved" });
        }

        // Admin rejects loan
        [Authorize(Roles = "Admin")]
        [HttpPost("{loanId}/reject")]
        public async Task<IActionResult> RejectLoan(Guid loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return NotFound();

            loan.Status = LoanStatus.Rejected;
            await _loanRepository.SaveChangesAsync();
            return Ok(new { message = "Loan rejected" });
        }

        // Admin disburses loan
        [Authorize(Roles = "Admin")]
        [HttpPost("{loanId}/disburse")]
        public async Task<IActionResult> DisburseLoan(Guid loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return NotFound();

            if (loan.Status != LoanStatus.Approved)
                return BadRequest("Loan must be approved before disbursement.");

            loan.Status = LoanStatus.Disbursed;
            loan.DisbursedAt = DateTime.UtcNow;

            await _loanRepository.SaveChangesAsync();
            return Ok(new { message = "Loan disbursed successfully." });
        }

        // User repays loan
        [HttpPost("{loanId}/repay")]
        public async Task<IActionResult> RepayLoan(Guid loanId, [FromBody] LoanRepaymentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null || loan.UserId != parsedUserId) return NotFound();

            if (loan.Status != LoanStatus.Disbursed)
                return BadRequest("Loan is not active for repayment.");

            if (dto.Amount <= 0)
                return BadRequest("Repayment amount must be greater than zero.");

            if (dto.Amount > loan.Balance)
                return BadRequest("Repayment amount cannot exceed the outstanding loan balance.");

            loan.Balance -= dto.Amount;

            if (loan.Balance <= 0)
            {
                loan.Status = LoanStatus.FullyRepaid;
                loan.Balance = 0;
            }

            await _loanRepository.SaveChangesAsync();

            return Ok(new { message = "Repayment recorded.", balance = loan.Balance, status = loan.Status });
        }

        // Get repayment schedule for a loan
        [HttpGet("{loanId}/schedule")]
        public async Task<IActionResult> GetRepaymentSchedule(Guid loanId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            // Make sure repository loads RepaymentSchedule collection
            var loan = await _loanRepository.GetByIdWithScheduleAsync(loanId);
            if (loan == null || loan.UserId != parsedUserId) return NotFound();

            return Ok(loan.RepaymentSchedule.Select(r => new
            {
                r.Id,
                r.DueDate,
                r.Amount,
                r.IsPaid
            }));
        }

        // Get current active loan
        [HttpGet("myloans/current")]
        public async Task<IActionResult> GetCurrentLoan()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || !Guid.TryParse(userId, out var parsedUserId))
                return Unauthorized();

            var loan = await _loanRepository.GetByUserIdWithScheduleAsync(parsedUserId);

            if (loan == null)
                return NotFound(new { message = "No active loan." });

            return Ok(new
            {
                loan.Id,
                loan.Status,
                loan.Balance,
                loan.MonthlyRepayment,
                loan.DisbursedAt,
                DueDates = loan.RepaymentSchedule?.Select(r => new
                {
                    r.DueDate,
                    r.Amount,
                    r.IsPaid
                }) ?? Enumerable.Empty<object>()
            });
        }

    }
}