using System;
using SyCoin.DataProvider;
using SyCoin.Models;

namespace SyCoin.Core
{
    public class UTXOManager
    {
        public IUTXODataProvider DataProvider { get; set; }

        public UTXOManager(IUTXODataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        internal UTXO GetUTXOInfo(string transactionHash, int outputIndex)
        {
            return DataProvider.GetUTXOInfo(transactionHash, outputIndex);
        }
    }
}
