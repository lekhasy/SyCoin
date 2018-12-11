using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SyCoin.Helpers;
using System.Linq;
using SyCoin.Models;
using SyCoin.DataProvider;

namespace SyCoin.Core
{
    public class SyCoinProtocol
    {
        IBlockDataProvider DataProvider;

        public SyCoinProtocol(IBlockDataProvider dataProvider)
        {
            DataProvider = dataProvider;

            // Add genesis block to the chain
            InitGenesisBlock();
        }

        public void InitGenesisBlock()
        {
            if (DataProvider.GetChainLength() != 0) return;

            SyCoinTransaction genesisData = new SyCoinTransaction
            {
                Sender = "Sy",
                Receiver = "Minh",
                Amount = 1000
            };

            var genesisBlock = new SyCoinBlock(new SyCoinTransaction[] { genesisData }, 1, "0000", DifficultTargetVerifier.GetCurrentDifficultTarger(DataProvider));
            var (nonce, timestamp) = FindNonce(genesisBlock);
            genesisBlock.Seal(nonce, timestamp);
            DataProvider.AddBlock(new PersistedBlock { Block = genesisBlock });
        }

        public SyCoinBlock MineNewBlock()
        {
            var previousHash = HashingHelper.HashObject(GetLatestBlock());
            var block = new SyCoinBlock(DataProvider.GetTopTransactionFeesExcept(1000,
                                        new string[] { }),
                                        (uint)DataProvider.GetChainLength() + 1,
                                        previousHash, DifficultTargetVerifier.GetCurrentDifficultTarger(DataProvider)
                                        );
            var (nonce, timestamp) = FindNonce(block);
            block.Seal(nonce, timestamp);
            DataProvider.AddBlock(new PersistedBlock { Block = block, Header = new BlockHeader()});
            return block;
        }

        public void AddTransaction(string sender, string receiver, decimal amount, decimal fee)
        {
            DataProvider.AddTransaction(new SyCoinTransaction() { Sender = sender, Receiver = receiver, Amount = amount, Fee = fee });
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

        private (uint nonce, long timestamp) FindNonce(SyCoinBlock block)
        {
            var clonedBlock = block.Clone();
            using (var nonceTimestampManipulator = new NonceTimestampManipulator())
            {
                while (true)
                {
                    clonedBlock.Nonce = nonceTimestampManipulator.GetNextNonce();
                    clonedBlock.Timestamp = nonceTimestampManipulator.CurrentTimestamp;
                    var blockHash = HashingHelper.HashObject(clonedBlock);
                    if (HashingHelper.IsHashMeetTarget(blockHash, DifficultTargetVerifier.GetCurrentDifficultTarger(DataProvider)))
                        return (clonedBlock.Nonce, clonedBlock.Timestamp);
                }
            }
        }

        public PersistedBlock GetLatestBlock() => DataProvider.GetLatestBlock();
        public PersistedBlock GetGenesisBlock() => DataProvider.GetGenesisBlock();
    }
}
