using System;
namespace SyCoin.Models
{
    public class UTXO
    {
        public string TransactionHash { get; set; }
        public int OutputIndex { get; set; }
    }
}
