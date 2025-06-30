using BankBlazor.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBlazor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly BankBlazorContext _context;

        public AccountsController(BankBlazorContext context)
        {
            _context = context;
        }

        // GET api/accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
            => await _context.Accounts.ToListAsync();

        // GET api/accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            var acc = await _context.Accounts.FindAsync(id);
            return acc == null ? NotFound() : acc;
        }

        // GET api/accounts/5/balance
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetBalance(int id)
        {
            var acc = await _context.Accounts.FindAsync(id);
            return acc == null ? NotFound() : acc.Balance;
        }

        // GET api/accounts/5/transactions
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(int id)
            => await _context.Transactions
                              .Where(t => t.AccountId == id)
                              .OrderByDescending(t => t.TransactionDate)
                              .ToListAsync();
    }
}
