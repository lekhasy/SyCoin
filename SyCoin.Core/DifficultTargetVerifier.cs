using System;
using System.Collections.Generic;
using SyCoin.DataProvider;

namespace SyCoin.Core
{
    public class DifficultTargetVerifier
    {
        IBlockDataProvider DataProvider;

        public DifficultTargetVerifier(IBlockDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        public uint GetCurrentDifficultTarget()
        {
            return 4;
        }
    }
}
