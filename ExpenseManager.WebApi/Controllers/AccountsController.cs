using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.WebApi.DatabContext;
using ExpenseManager.WebApi.Models;

namespace ExpenseManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
        [HttpGet("GetCurrentMoney/{id}")]
        public async Task<ActionResult<int>> GetCurrentMoney(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) { return NotFound();}
            return Ok(account.CurrentMoney);
        }
        [HttpGet("GetConsolidatedMoney/{id}")]
        public async Task<ActionResult<int>> GetConsolidatedMoney(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) { return NotFound(); }
            return Ok(account.MoneySpent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(Guid id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            var existingAccount = await _context.Accounts.FindAsync(id);
            if (existingAccount == null) { return NotFound(); }
            
           
            existingAccount.UserName = account.UserName;
        
            await _context.SaveChangesAsync();
           
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostAccount(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
