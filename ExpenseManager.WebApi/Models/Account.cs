using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Primitives;
namespace ExpenseManager.WebApi.Models
{
    public class Account
    {
        public string UserName { get; set; }


        // id provicional = EF926559-2FB8-460D-9B42-22BA4CE934E1
        public Guid Id { get; set; }
        public int CurrentMoney { get; set; }
       
        public int MoneySpent { get; set; }
    }


    
}
