using System;
using SyCoin.Models;
using System.Collections.Generic;

namespace SyCoin.DataProvider
{
    public interface IBlockDataProvider
    {
        void AddBlock(PersistedBlock newBlock);
        void AddTransaction(SyCoinTransaction transaction);
        long GetChainLength();
        PersistedBlock GetLatestBlock();
        PersistedBlock GetGenesisBlock();
        IEnumerable<SyCoinTransaction> GetTopTransactionFeesExcept(uint max_transaction, IEnumerable<string> exceptedTransactionIds);
        IEnumerable<PersistedBlock> GetChainPart(uint start, uint limit);
    }
}
