namespace BankBlazor.API.Models.Dto
{
    public class DepositRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
