using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SyCoin.Models;

namespace SyCoin.DataProvider.Mongo
{
    public class MongoUTXODataProvider : MongoDataProvider, IUTXODataProvider
    {
        public MongoUTXODataProvider(IOptions<AppSettingModel> options) : base(options) {}

        IMongoCollection<UTXO> GetUTXOCollection()
        {
            return GetDatabase().GetCollection<UTXO>(AppSetting.MongoDb.UTXOCollectionName);
        }

        public UTXO GetUTXOInfo(string transactionHash, int outputIndex)
        {
            return GetUTXOCollection()
                    .Find(utxo => utxo.TransactionHash == transactionHash && utxo.OutputIndex == outputIndex)
                    .FirstOrDefault();
        }
    }
}
