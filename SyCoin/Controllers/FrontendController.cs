using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyCoin.Core;
using SyCoin.DataProvider;
using SyCoin.Models;
using SyCoin.API.Models;

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

        [HttpPost]
        public void AddTransaction(AddTransactionModel model)
        {
            //CoinProtocol.AddTransaction()
        }

        [HttpGet]
        public IEnumerable<PersistedBlock> GetChainPart(uint start, uint limit)
        {
            return DataProvider.GetChainPart(start, limit);
        }

        //[HttpPost]
        //public void AddTransaction(sender, receiver, )
    }
}
