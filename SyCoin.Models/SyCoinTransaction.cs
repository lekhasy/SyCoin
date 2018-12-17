namespace SyCoin.Models
{
    public class SycoinTransactionContent
    {
        public TransactionInput[] Input { get; set; }
        public TransactionOutput[] Outputs { get; set; }
    }

    public class SyCoinTransaction
    {
        public SycoinTransactionContent Content { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }
        public string PublicKey { get; set; }
    }

    public class TransactionOutput
    {
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactionInput
    {
        public string TransactionHash { get; set; }
        public int PrevOutputIndex { get; set; }
    }
}
