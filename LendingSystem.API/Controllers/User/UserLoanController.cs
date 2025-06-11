using System.Security.Claims;
using LendingSystem.API.DTOs.Loan;
using LendingSystem.Entities;
using LendingSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LendingSystem.API.Controllers.User;

[ApiController]
[Authorize]
[Route("api/user/loans")]
public class UserLoanController : ControllerBase
{
    private readonly ILoanRepository _loanRepository;

    public UserLoanController(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] LoanApplyDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var parsedUserId)) return Unauthorized();

        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            UserId = parsedUserId,
            Amount = dto.Amount,
            TermMonths = dto.TermMonths,
            Status = LoanStatus.Pending,
            AppliedAt = DateTime.UtcNow
        };

        loan.InitializeRepayment(10); // 10% interest
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

    [HttpGet("mine")]
    public async Task<IActionResult> GetMyLoans()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var parsedUserId)) return Unauthorized();

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

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentLoan()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var parsedUserId)) return Unauthorized();

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
            DueDates = loan.RepaymentSchedule.Select(r => new
            {
                r.DueDate,
                r.Amount,
                r.IsPaid
            })
        });
    }

    // New schedule endpoint for user

    // GET api/user/loans/{loanId}/schedule
    [HttpGet("{loanId}/schedule")]
    public async Task<IActionResult> GetRepaymentSchedule(Guid loanId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var parsedUserId)) return Unauthorized();

        var loan = await _loanRepository.GetByIdWithScheduleAsync(loanId);
        if (loan == null || loan.UserId != parsedUserId) return NotFound();

        return Ok(loan.RepaymentSchedule.Select(r => new
        {
            r.Id,
            r.DueDate,
            r.Amount,
            r.IsPaid,
            r.PaidDate
        }));
    }
}
