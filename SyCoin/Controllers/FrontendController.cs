using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyCoin.Core;
using SyCoin.DataProvider;
using SyCoin.Models;

namespace SyCoin.API.Controllers
{
    [Route("api/[controller]")]
    public class FrontendController : Controller
    {
        SyCoinProtocol CoinProtocol;
        IBlockDataProvider DataProvider;

        public FrontendController(SyCoinProtocol coinProtocol, IBlockDataProvider dataProvider)
        {
            CoinProtocol = coinProtocol;
            DataProvider = dataProvider;
        }

        [HttpGet]
        public IEnumerable<PersistedBlock> GetChainPart(uint start, uint limit)
        {
            return DataProvider.GetChainPart(start, limit);
        }
    }
}
