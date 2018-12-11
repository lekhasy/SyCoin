namespace SyCoin.Models
{
    public class SyCoinTransaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
    }
}
