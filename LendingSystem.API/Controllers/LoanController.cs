using LendingSystem.API.DTOs;
using LendingSystem.Entities;
using LendingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (userId == null) return Unauthorized();

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                Amount = dto.Amount,
                TermMonths = dto.TermMonths,
                Status = LoanStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

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

        // User views their loan status
        [HttpGet("myloans")]
        public async Task<IActionResult> GetMyLoans()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var loans = await _loanRepository.GetByUserIdAsync(Guid.Parse(userId));
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

        // Admin approves or rejects loan
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
    }
}
