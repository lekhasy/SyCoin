using System.Collections.Generic;

namespace SyCoin.Models
{
    public class SyCoinBlock : Block<IEnumerable<SyCoinTransaction>>
    {
        public SyCoinBlock(IEnumerable<SyCoinTransaction> block_data, uint block_index, string previous_hash, uint target) : base(block_data, block_index, previous_hash, target)
        {

        }
    }
}
