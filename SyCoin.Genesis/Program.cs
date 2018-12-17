using System;
using SyCoin.Helpers;
using System.Security.Cryptography;
using System.IO;
using SyCoin.Models;
using SyCoin.Core.Miner;

namespace SyCoin.Genesis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");
            var rsa = RSA.Create();
            var key = rsa.ExportParameters(true);

            var ZeroHexHash = "";
            for (int i = 0; i < 64; i++)
                ZeroHexHash += "0";

            Console.WriteLine("Generating key files");

            #region Save Public Key
            string publicKey = RSAHelper.ExportPublicKeyToPEMFormat(rsa);

            FileInfo pem = new FileInfo("public.pem");
            if (pem.Exists)
                pem.Delete();

            using (var stream = pem.OpenWrite())
            using (var writer = new StreamWriter(stream))
                writer.Write(publicKey);
            #endregion

            #region Save Private Key

            string privateKey = RSAHelper.ExportPrivateKeyToPfxFormat(rsa);

            FileInfo pfx = new FileInfo("private.pfx");
            if (pfx.Exists)
                pfx.Delete();

            using (var stream = pfx.OpenWrite())
            using (var writer = new StreamWriter(stream))
                writer.Write(privateKey);

            #endregion

            Console.WriteLine("Public and private key file saved");

            Console.WriteLine("Mining Genesis block");
            #region Generate Genesis Block

            var transactionContent = new SycoinTransactionContent()
            {
                Outputs = new TransactionOutput[]
                {
                    new TransactionOutput
                    {
                        Amount = 50,
                        Receiver = publicKey
                    }
                }
            };

            var contentHash = HashingHelper.HashObject(transactionContent);

            var transactions = new SyCoinTransaction[]
            {
                new SyCoinTransaction
                {
                    Content = transactionContent,
                    Hash = HashingHelper.ByteArrayToHexDigit(contentHash)
                }
            };

            var blockData = new SyCoinBlock(transactions, 1, ZeroHexHash, 4);

            var (nonce, timestamp) = new BlockMiner().Mine(blockData);

            blockData.Seal(nonce, timestamp);

            PersistedBlock genesisBlock = new PersistedBlock()
            {
                Header = new BlockHeader(),
                Block = blockData
            };

            FileInfo genesisJson = new FileInfo("genesis.json");
            if (genesisJson.Exists)
                genesisJson.Delete();

            using (var stream = genesisJson.OpenWrite())
            using (var writer = new StreamWriter(stream))
                writer.Write(Newtonsoft.Json.JsonConvert.SerializeObject(genesisBlock));

            #endregion

            Console.WriteLine("Genesis block saved in genesis.json");
            Console.ReadLine();
        }
    }
}
