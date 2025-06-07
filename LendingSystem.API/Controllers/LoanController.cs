using LendingSystem.Interfaces;
using LendingSystem.Entities;
using LendingSystem.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LendingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForLoan(LoanApplyDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var loan = new Loan
            {
                Amount = dto.Amount,
                AppliedOn = DateTime.UtcNow,
                Status = "Pending",
                BorrowerId = userId
            };

            await _loanRepository.AddLoanAsync(loan);

            return Ok(new { loan.Id, loan.Status });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyLoans()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var loans = await _loanRepository.GetLoansByUserIdAsync(userId);

            return Ok(loans);
        }

        // Add admin actions to approve/reject loans here...
    }
}
