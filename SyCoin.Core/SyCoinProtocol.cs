using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SyCoin.Helpers;
using System.Linq;
using SyCoin.Models;
using SyCoin.DataProvider;
using SyCoin.Core.Miner;
using System.IO;
using System.Reflection;

namespace SyCoin.Core
{
    public class SyCoinProtocol
    {
        IBlockDataProvider DataProvider;
        UTXOManager UTXOManager;
        BlockMiner BlockMiner = new BlockMiner();
        DifficultTargetVerifier DifficultTargetVerifier;

        public SyCoinProtocol(IBlockDataProvider dataProvider, UTXOManager utxoManager)
        {
            DataProvider = dataProvider;
            UTXOManager = utxoManager;
            DifficultTargetVerifier = new DifficultTargetVerifier(dataProvider);

            // Add genesis block to the chain
            InitGenesisBlock();
        }

        public void InitGenesisBlock()
        {
            if (DataProvider.GetChainLength() != 0) return;

            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = buildDir + @"/GenesisBlock.json";

            // read genesis block from GenesisBlock.json file
            FileInfo gnsFile = new FileInfo(filePath);
            using(var reader = gnsFile.OpenText())
            {
                var jsonContent = reader.ReadToEnd();
                var GenesisBlock = JsonConvert.DeserializeObject<PersistedBlock>(jsonContent);
                DataProvider.AddBlock(GenesisBlock);
            }
        }

        public SyCoinBlock MineNewBlock()
        {
            var previousHash = HashingHelper.ByteArrayToHexDigit(HashingHelper.HashObject(GetLatestBlock()));
            var block = new SyCoinBlock(DataProvider.GetTopTransactionFeesExcept(1000,
                                        new string[] { }),
                                        (uint)DataProvider.GetChainLength() + 1,
                                        previousHash, DifficultTargetVerifier.GetCurrentDifficultTarget());

            var (nonce, timestamp) = BlockMiner.Mine(block);
            block.Seal(nonce, timestamp);
            DataProvider.AddBlock(new PersistedBlock { Block = block, Header = new BlockHeader() });
            return block;
        }

        public void AddTransaction(SycoinTransactionContent transactionContent, string privateKey)
        {
            #region Verify transaction input and output

            // Input UTXO check
            transactionContent.Input.All(input =>
            {
                var UTXOInfo = UTXOManager.GetUTXOInfo(input.TransactionHash, input.PrevOutputIndex);
                if (UTXOInfo != null) throw new Exceptions.NonUTXOException(input);
                return true;
            });

            var allInputTransactions = DataProvider.GetTransactions(transactionContent.Input.Select(i => i.TransactionHash));

            #region Output validation
            var inputAmount = transactionContent.Input.Select(input =>
            {
                var prev_trans = allInputTransactions.FirstOrDefault(x => x.Hash == input.TransactionHash);
                var prev_output = prev_trans.Content.Outputs.ElementAt(input.PrevOutputIndex);
                return prev_output.Amount;
            }).Sum();

            var outputAmount = transactionContent.Outputs.Sum(output => output.Amount);

            if (inputAmount < outputAmount) throw new Exceptions.InputNotEnoughTokenException();
            #endregion

            #endregion

            var contentHash = HashingHelper.HashObject(transactionContent);
            var rsa = RSAHelper.CreateRsaProviderFromPrivateKey(privateKey);
            var publicKey = RSAHelper.ExportPublicKeyToPEMFormat(rsa);

            DataProvider.AddTransaction(new SyCoinTransaction()
            {
                Signature = HashingHelper.ByteArrayToHexDigit(DigitalSignatureHelper.SignHash(contentHash, rsa.ExportParameters(true))),
                Hash = HashingHelper.ByteArrayToHexDigit(contentHash),
                PublicKey = publicKey,
                Content = transactionContent
            });
        }

        //public bool IsChainValid(IBlockDataProvider chain)
        //{
        //    // validate genesisblock
        //    if (!HashingHelper.IsHashMeetTarget(
        //            HashingHelper.HashObject(chain[0]),
        //            DifficultTargetVerifier.GetCurrentDifficultTarger(Chain)))
        //        return false;

        //    // validate the rest
        //    var previousBlock = chain.First();
        //    var blockIndex = 1;
        //}

        public PersistedBlock GetLatestBlock() => DataProvider.GetLatestBlock();
        public PersistedBlock GetGenesisBlock() => DataProvider.GetGenesisBlock();
    }
}
