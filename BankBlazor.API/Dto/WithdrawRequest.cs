namespace BankBlazor.API.Dto
{
    public class WithdrawRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }

        // You can add validation attributes if needed, e.g.:
        // [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        // public decimal Amount { get; set; }
    }
}
