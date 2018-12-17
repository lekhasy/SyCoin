using System;
using SyCoin.Helpers;
using SyCoin.Models;

namespace SyCoin.Core.Miner
{
    public class BlockMiner
    {
        public (uint nonce, long timestamp) Mine(SyCoinBlock block)
        {
            var clonedBlock = block.Clone();
            using (var nonceTimestampManipulator = new NonceTimestampManipulator())
            {
                while (true)
                {
                    clonedBlock.Nonce = nonceTimestampManipulator.GetNextNonce();
                    clonedBlock.Timestamp = nonceTimestampManipulator.CurrentTimestamp;
                    var blockHash = HashingHelper.ByteArrayToHexDigit(HashingHelper.HashObject(clonedBlock));
                    if (HashingHelper.IsHashMeetTarget(blockHash, block.Target))
                        return (clonedBlock.Nonce, clonedBlock.Timestamp);
                }
            }
        }
    }
}
