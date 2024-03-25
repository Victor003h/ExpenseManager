using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.WebApi.DatabContext;
using ExpenseManager.WebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpenseManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        [HttpGet("GetExpenseById/{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        [HttpGet("GetExpensesByLabel/{label}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesByLabel(string label)
        {
            var res = await _context.Expenses.Where(egreso => egreso.Label == label).ToListAsync();
            if (res == null)
                return NotFound();

            return res;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(Guid id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }

            var existingEgreso = await _context.Expenses.FindAsync(id);
            if(existingEgreso == null) { return  NotFound(); }
            if (existingEgreso.Consolidated == 1)
            {
                return Unauthorized("This expense is already consolidated");
            }
          
            
            existingEgreso.Date = DateTime.Now;
            existingEgreso.Description = expense.Description;
            existingEgreso.Amount = expense.Amount;
            existingEgreso.Label = expense.Label;
            await _context.SaveChangesAsync();
            
            return  NoContent(); 

        }

        [HttpPut("Consolidate/{id}")]
        public async Task<IActionResult> ConsolidatedExp(Guid id)
        {
            var existingEgreso = await _context.Expenses.FindAsync(id);
            if (existingEgreso == null) { return NotFound(); }
            existingEgreso.Consolidated = 1;
            var account = _context.Accounts.FindAsync(new Guid("EF926559-2FB8-460D-9B42-22BA4CE934E1")).Result;
            if (account != null)
                account.MoneySpent += existingEgreso.Amount;
            await _context.SaveChangesAsync();
               
            return NoContent();
        }


        [HttpPost("{amount}/{description}/{label}")]
        public async Task<IActionResult> PostExpense(int amount,string description,string label)
        {
            
            var expense= new Expense() { Amount=amount, Description=description, Label=label ,
                Date=DateTime.Today,Id=Guid.NewGuid(),Consolidated=0};

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
