using System;

namespace SyCoin.Models
{
    public class PersistedBlock
    {
        public SyCoinBlock Block { get; set; }
        public BlockHeader Header { get; set; }
    }
}
