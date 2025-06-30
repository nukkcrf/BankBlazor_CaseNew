using BankBlazor.API.Dto;
using BankBlazor.API.Models;
using BankBlazor.API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBlazor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly BankBlazorContext _context;

        public TransactionsController(BankBlazorContext context)
        {
            _context = context;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest req)
        {
            var account = await _context.Accounts.FindAsync(req.AccountId);
            if (account == null) return NotFound("Account not found");

            account.Balance += req.Amount;

            var tx = new Transaction
            {
                AccountId = req.AccountId,
                Amount = req.Amount,
                TransactionDate = DateTime.UtcNow,
                Type = "Deposit"
            };
            _context.Transactions.Add(tx);

            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest req)
        {
            var account = await _context.Accounts.FindAsync(req.AccountId);
            if (account == null) return NotFound("Account not found");
            if (account.Balance < req.Amount)
                return BadRequest("Insufficient funds");

            account.Balance -= req.Amount;

            var tx = new Transaction
            {
                AccountId = req.AccountId,
                Amount = -req.Amount,
                TransactionDate = DateTime.UtcNow,
                Type = "Withdraw"
            };
            _context.Transactions.Add(tx);

            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest req)
        {
            if (req.FromAccountId == req.ToAccountId)
                return BadRequest("Cannot transfer to same account");

            var from = await _context.Accounts.FindAsync(req.FromAccountId);
            var to = await _context.Accounts.FindAsync(req.ToAccountId);
            if (from == null || to == null) return NotFound("One or both accounts not found");
            if (from.Balance < req.Amount)
                return BadRequest("Insufficient funds");

            using var txDb = await _context.Database.BeginTransactionAsync();
            try
            {
                from.Balance -= req.Amount;
                to.Balance += req.Amount;

                _context.Transactions.Add(new Transaction
                {
                    AccountId = from.AccountId,
                    Amount = -req.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Type = "TransferOut",
                   
                });
                _context.Transactions.Add(new Transaction
                {
                    AccountId = to.AccountId,
                    Amount = req.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Type = "TransferIn",
                   
                });

                await _context.SaveChangesAsync();
                await txDb.CommitAsync();
                return Ok(new { From = from, To = to });
            }
            catch
            {
                await txDb.RollbackAsync();
                throw;
            }
        }
    }
}
