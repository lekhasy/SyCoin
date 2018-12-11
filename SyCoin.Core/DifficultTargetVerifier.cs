using System;
using System.Collections.Generic;
using SyCoin.DataProvider;

namespace SyCoin.Core
{
    internal static class DifficultTargetVerifier
    {
        public static uint GetCurrentDifficultTarger(IBlockDataProvider chain)
        {
            return 4;
        }
    }
}
