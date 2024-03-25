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
    public class IncomesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IncomesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes()
        {
            return await _context.Incomes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Income>> GetIncome(Guid id)
        {
            var income = await _context.Incomes.FindAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            return income;
        }

        [HttpPost("{amount}")]
        public async Task<IActionResult> PostIncome(int amount)
        {
            var income = new Income() { Amount = amount, Date = DateTime.Now, Id = Guid.NewGuid() };

            var account = _context.Accounts.FindAsync(new Guid("EF926559-2FB8-460D-9B42-22BA4CE934E1")).Result;
            if (account != null)
                account.CurrentMoney += amount;

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(Guid id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
