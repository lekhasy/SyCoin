using System;

namespace SyCoin.Models
{
    public class AppSettingModel
    {
        public MongoDbSetting MongoDb { get; set; }
    }

    public class MongoDbSetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DbName { get; set; }
        public string LedgerCollectionName { get; set; }
        public string MempoolCollectionName { get; set; }
    }
}
