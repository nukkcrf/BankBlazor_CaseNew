// BankBlazor.Client/Models/TransactionDto.cs
namespace BankBlazor.Client.Models
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = default!;
        public string Operation { get; set; } = default!;     // Lägg till
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }                  // Lägg till
        public int AccountId { get; set; }
    }
}
