using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using esstp.Models.Services;
using esstp.Models.ViewModels;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using esstp.Models;
using Newtonsoft.Json;
using System.Linq;

namespace esstp.Controllers
{
     
    //[Authorize]
    //[Produces("application/json")]
    //[ApiController]
    //[Route("api/[controller]")]
    public class MarketController : Controller
    {
        private IMarketService _marketService;
        private IAssetService _assetService;
        private IMapper _mapper;

        public MarketController(
            IMarketService marketService,
            IAssetService assetService,
            IMapper mapper
            )
        {
            _marketService = marketService;
            _assetService = assetService;
            _mapper = mapper;
        }

     


        //GET /Market/ReadById/:id

        //[Authorize(Policy = "market-readbyid")]
        //[HttpGet, Route("ReadById/{id}")]
        public async Task<IActionResult> ReadById(int id)
        {
            //var dbItem = await _userService.FindAllAsync(j => j.Id == id);
            var dbItem = await _marketService.FindAsync(j => j.Id == id);
            //404
            if (dbItem == null)
                return NotFound();

            //var userDtos = _mapper.Map<IList<UserDto>>(dbItem);
            var marketDtos = _mapper.Map<MarketViewModel>(dbItem);
            return View(marketDtos);
        }



        // GET /Market/ReadAll

        //ReadAll
        //[Authorize(Policy = "market-readall")]
        [HttpGet("ReadAll")]
        public async Task<IActionResult> ReadAll()
        {
            var market = await _marketService.GetAllAsync();
            var marketDto = _mapper.Map<IList<Market>>(market);
            var asset = await _assetService.GetAllAsync();
            var assetDto = _mapper.Map<IList<Asset>>(asset);

            var marketViewModelDto = from m in marketDto
                                     join a in assetDto on m.Asset_id equals a.Id into table1
                                     from a in table1.ToList()
                                     select new MarketViewModel
                                     {
                                         Id = m.Id,
                                         Isin = m.Isin,
                                         Name = m.Name,
                                         Symbol = m.Symbol,
                                         Buying = m.Buying,
                                         Selling = m.Selling,
                                         Asset = a.Name 
                                     };

            return View(marketViewModelDto);
            
        }




        // GET: Create
        public async Task<IActionResult> Buy(int id)
        {
            //var dbItem = await _userService.FindAllAsync(j => j.Id == id);
            var dbItem = await _marketService.FindAsync(j => j.Id == id);
            //404
            if (dbItem == null)
                return NotFound();
   
            var marketDtos = _mapper.Map<MarketViewModel>(dbItem);
            var BrokerMP = Convert.ToDouble(AppConfig.Config["AppSettings:brokerMarginInPercent"]);


            PortfolioPositionViewModel position = new PortfolioPositionViewModel()
            {
                Market_Id = marketDtos.Id,
                Name = marketDtos.Name,
                Symbol = marketDtos.Symbol,
                ValueOpen = Math.Round(marketDtos.Buying * (1 + BrokerMP), 2, MidpointRounding.AwayFromZero),
                Cfd_id = 1,
                Invested = 0,
                StopLoss = 0,
                TakeProfit = 0,
                BrokerMargin = BrokerMP,
                
            };
   
            TempData["position"] = JsonConvert.SerializeObject(position);
            return RedirectToAction("Buy", "Portfolio"); 
        }



        // GET: Create
        public async Task<IActionResult> Sell(int id)
        {
            var dbItem = await _marketService.FindAsync(j => j.Id == id);
            //404
            if (dbItem == null)
                return NotFound();

            var marketDtos = _mapper.Map<MarketViewModel>(dbItem);

            PortfolioPositionViewModel position = new PortfolioPositionViewModel()
            {
                Market_Id = marketDtos.Id,
                Name = marketDtos.Name,
                Symbol = marketDtos.Symbol,
                ValueOpen = Math.Round(marketDtos.Selling, 2, MidpointRounding.AwayFromZero),
                Cfd_id = 2,
                Invested = 0,
                StopLoss = 0,
                TakeProfit = 0
            };

            TempData["position"] = JsonConvert.SerializeObject(position);
            return RedirectToAction("Sell", "Portfolio");
        }



    }
}





//Unit Calculation
//In order to calculate the amount of units for a trade, we may use one of the formulas below,
//according to the instrument being traded.
//Please note that, previously, the unit and margin calculation for both BUY and SELL trades
//was based only on the SELL (Bid) rate.
//As of 21 February 2018, the unit amount for BUY trades is calculated based on the BUY (Ask)
//rate to better reflect asset ownership.For SELL trades, it is still based on the SELL (Bid) rate.




//Currencies, Commodities, American Stocks and Cryptocurrencies:

//[Amount Invested] X[Leverage] / [Rate when position opened] = Units

//Examples:

//A SELL Gold trade with $1000 invested and x10 leverage opened at a price of $1330.55 = 7.52 units
//From 21/02/2018, a BUY Gold trade with $1000 invested and x10 leverage opened at a price of $1331.00 = 7.51 units