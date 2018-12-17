using System;
using System.Collections.Generic;
using MongoDB.Driver;
using SyCoin.Models;
using System.Linq;
using Microsoft.Extensions.Options;

namespace SyCoin.DataProvider.Mongo
{
    public class MongoBlockDataProvider : MongoDataProvider, IBlockDataProvider
    {
        public MongoBlockDataProvider(IOptions<AppSettingModel> options) : base(options) { }

        IMongoCollection<PersistedBlock> GetLedgerCollection()
        {
            return GetDatabase().GetCollection<PersistedBlock>(AppSetting.MongoDb.LedgerCollectionName);
        }

        IMongoCollection<SyCoinTransaction> GetMemPoolCollection()
        {
            return GetDatabase().GetCollection<SyCoinTransaction>(AppSetting.MongoDb.MempoolCollectionName);
        }

        public void AddBlock(PersistedBlock newBlock)
        {
            GetLedgerCollection().InsertOne(newBlock);
        }

        public long GetChainLength()
        {
            return GetLedgerCollection().CountDocuments(bl => true);
        }

        public void AddTransaction(SyCoinTransaction transaction)
        {
            GetMemPoolCollection().InsertOne(transaction);
        }

        public PersistedBlock GetLatestBlock()
        {
            return GetLedgerCollection().Find(x => true).SortByDescending(o => o.Block.Index).First();
        }

        public PersistedBlock GetGenesisBlock()
        {
            return GetLedgerCollection().Find(x => x.Block.Index == 1).First();
        }

        public IEnumerable<SyCoinTransaction> GetTopTransactionFeesExcept(uint max_transaction, IEnumerable<string> exceptedTransactionIds)
        {
            return GetMemPoolCollection().Find(x => true).ToList();
        }

        public IEnumerable<PersistedBlock> GetChainPart(uint start, uint limit)
        {
            return GetLedgerCollection().Find(x => true).SortBy(x => x.Block.Index).Skip((int)start).Limit((int)limit).ToList();
        }

        public PersistedBlock GetBlock(uint index)
        {
            return GetLedgerCollection().Find(x => index == x.Block.Index).First();
        }

        public IEnumerable<PersistedBlock> GetBlocks(IEnumerable<uint> indexes)
        {
            return GetLedgerCollection().Find(x => indexes.Contains(x.Block.Index)).ToList();
        }

        public IEnumerable<SyCoinTransaction> GetTransactions(IEnumerable<string> transactionHashes)
        {
            var transactionsInChain = GetLedgerCollection()
                                        .Find(x => x.Block.Data.Any(trans => transactionHashes.Contains(trans.Hash))).ToList()
                                        .SelectMany(bl => bl.Block.Data).Where(trans => transactionHashes.Contains(trans.Hash));

            var transactionsInMempool = GetMemPoolCollection()
                                        .Find(trans => transactionHashes.Contains(trans.Hash)).ToList();

            return transactionsInChain.Concat(transactionsInMempool);
        }
    }
}
