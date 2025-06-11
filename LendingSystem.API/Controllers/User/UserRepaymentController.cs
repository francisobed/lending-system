using System.Security.Claims;
using LendingSystem.API.DTOs.Loan;
using LendingSystem.Entities;
using LendingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LendingSystem.API.Controllers.User;

[ApiController]
[Authorize]
[Route("api/user/repayments")]
public class UserRepaymentController : ControllerBase
{
    private readonly ILoanRepository _loanRepository;

    public UserRepaymentController(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    [HttpPost("{loanId}")]
    public async Task<IActionResult> RepayLoan(Guid loanId, [FromBody] LoanRepaymentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var parsedUserId)) return Unauthorized();

        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null || loan.UserId != parsedUserId) return NotFound();

        if (loan.Status != LoanStatus.Disbursed)
            return BadRequest("Loan is not active for repayment.");

        if (dto.Amount <= 0 || dto.Amount > loan.Balance)
            return BadRequest("Invalid repayment amount.");

        loan.Balance -= dto.Amount;
        if (loan.Balance <= 0)
        {
            loan.Balance = 0;
            loan.Status = LoanStatus.FullyRepaid;
        }

        await _loanRepository.SaveChangesAsync();
        return Ok(new { message = "Repayment recorded.", balance = loan.Balance, status = loan.Status });
    }
}
