using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SyCoin.Models;

namespace SyCoin.DataProvider
{
    public class MongoBlockDataProvider : IBlockDataProvider
    {
        private AppSettingModel AppSetting;
        public MongoBlockDataProvider(IOptions<AppSettingModel> options)
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("My convention", pack, t => true);

            AppSetting = options.Value;
        }

        MongoClient GetClient()
        {
            return new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(AppSetting.MongoDb.Host, AppSetting.MongoDb.Port)
            });
        }

        IMongoDatabase GetDatabase()
        {
            return GetClient().GetDatabase(AppSetting.MongoDb.DbName);
        }

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
    }
}
