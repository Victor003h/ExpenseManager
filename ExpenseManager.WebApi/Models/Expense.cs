namespace ExpenseManager.WebApi.Models
{
    public class Expense
    {

        public Guid Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }
        public string Label { get; set; }

        public int Consolidated { get; set; }
    }
}
