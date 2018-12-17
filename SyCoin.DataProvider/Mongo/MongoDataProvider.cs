using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SyCoin.Models;

namespace SyCoin.DataProvider.Mongo
{
    public abstract class MongoDataProvider
    {
        protected AppSettingModel AppSetting;
        protected MongoDataProvider(IOptions<AppSettingModel> options)
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("My convention", pack, t => true);

            AppSetting = options.Value;
        }

        protected MongoClient GetClient()
        {
            return new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(AppSetting.MongoDb.Host, AppSetting.MongoDb.Port)
            });
        }

        protected IMongoDatabase GetDatabase()
        {
            return GetClient().GetDatabase(AppSetting.MongoDb.DbName);
        }
    }
}
