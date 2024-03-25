using ExpenseManager.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.WebApi.DatabContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}

