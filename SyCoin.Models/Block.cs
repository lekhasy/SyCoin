using System;

namespace SyCoin.Models
{
    public class Block<T>
    {
        public uint Index { get; set; }
        public T Data { get; set; }
        public string PreviousHash { get; set; }
        public uint Nonce { get; set; }
        public long Timestamp { get; set; }
        public uint Target { get; set; }

        public Block(T block_data, uint block_index, string previous_hash, uint target)
        {
            Index = block_index;
            Data = block_data;
            PreviousHash = previous_hash;
            Target = target;
            Nonce = 0;
            Timestamp = 0;
        }

        public void Seal(uint nonce, long timestamp)
        {
            Nonce = nonce;
            Timestamp = timestamp;
        }

        public Block<T> Clone()
        {
            var cloned_block = new Block<T>(Data, Index, PreviousHash, Target)
            {
                Nonce = Nonce,
                Timestamp = Timestamp
            };
            return cloned_block;
        }
    }
}