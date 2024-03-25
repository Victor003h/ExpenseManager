namespace ExpenseManager.WebApi.Models
{
    public class Income
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
