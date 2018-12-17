using SyCoin.Models;

namespace SyCoin.DataProvider
{
    public interface IUTXODataProvider
    {
        UTXO GetUTXOInfo(string transactionHash, int outputIndex);
    }
}
