using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using esstp.Models;
using esstp.Models.Services;
using esstp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace esstp.Controllers
{
    public class PortfolioController : Controller
    {
       //[Authorize]
    //[Produces("application/json")]
    //[ApiController]
    //[Route("api/[controller]")]
    
        private IPortfolioService _portfolioService;
        private IMarketService _marketService;
        private ICfdService _cfdService;
        private IOperationService _operationService;
        private IMapper _mapper;
        private ConcreteSubject _subject;

        public PortfolioController(
            IPortfolioService portfolioService,
            IMarketService marketService,
            ICfdService cfdService,
            IOperationService operationService,
            IMapper mapper,
             ConcreteSubject subject
            )
        {
            _portfolioService = portfolioService;
            _marketService = marketService;
            _cfdService = cfdService;
            _operationService = operationService;
            _mapper = mapper;
            _subject = subject;
        }




        //GET /Portfolio/ReadById/:id

        //[Authorize(Policy = "portfolio-readbyid")]
        //[HttpGet, Route("ReadById/{id}")]
        public async Task<IActionResult> ReadById(int id)
        {         
            var p = await _portfolioService.FindAsync(j => j.Id == id);
            var position = _mapper.Map<PortfolioPositionViewModel>(p);
            var m = await _marketService.FindAsync(j => j.Id == p.Market_id);
            var c = await _cfdService.FindAsync(j => j.Id == p.Cfd_id);
            
            position.Name = m.Name;
            position.Symbol = m.Symbol;
            position.Position = c.Name;

            position.Current = p.Cfd_id != 2 ? m.Buying : m.Selling;
            position.CurrentProfit = p.Cfd_id != 2 ? (m.Buying  - p.ValueOpen) * (p.Invested / p.ValueOpen) : (p.ValueOpen - m.Selling) * (p.Invested / p.ValueOpen);

            return View(position);

        }



        // GET /portfolio/ReadPortfolio

        //ReadPortfolio
        //[Authorize(Policy = "portfolio-readall")]
        //[HttpGet("ReadPortfolio")]
        public async Task<IActionResult> ReadPortfolio()
        {
            var portfolio = await _portfolioService.GetAllAsync();
            var portfolioDto = _mapper.Map<IList<Portfolio>>(portfolio);
            var market = await _marketService.GetAllAsync();
            var marketDto = _mapper.Map<IList<Market>>(market);
            var cfd = await _cfdService.GetAllAsync();
            var cfdDto = _mapper.Map<IList<Cfd>>(cfd);

            var portfolioPageDto = from p in portfolioDto 
                                    join m in marketDto on p.Market_id  equals m.Id  into table1
                                    from m in table1.ToList()
                                    join c in cfdDto on p.Cfd_id equals c.Id into table2
                                    from c in table2.ToList()
                                    where p.State == 0
                                    select new PortfolioPageViewModel
                                    {
                                        Id = p.Id,
                                        Position = c.Name,
                                        Name = m.Name,
                                        Symbol = m.Symbol,
                                        Units = p.Units,
                                        ValueOpen = p.ValueOpen,
                                        Invested = p.Invested,

                                        CurrentProfit = p.Cfd_id  != 2 ?  (m.Buying - p.ValueOpen)*(p.Invested/p.ValueOpen) : ( p.ValueOpen - m.Selling) * (p.Invested/p.ValueOpen),
                                        CurrentInvested = p.Cfd_id != 2 ? p.Invested + (m.Buying - p.ValueOpen) * (p.Invested / p.ValueOpen) : p.Invested + (p.ValueOpen - m.Selling) * (p.Invested / p.ValueOpen),
                                        Current = p.Cfd_id != 2 ? m.Buying : m.Selling
                                    };
            
            return View(portfolioPageDto);
        }


        //ReadHistory
        //[Authorize(Policy = "portfolio-readhistory")]
        //[HttpGet("ReadHistory")]
        public async Task<IActionResult> ReadHistory()
        {
            var portfolio = await _portfolioService.GetAllAsync();
            var portfolioDto = _mapper.Map<IList<Portfolio>>(portfolio);
            var market = await _marketService.GetAllAsync();
            var marketDto = _mapper.Map<IList<Market>>(market);
            var cfd = await _cfdService.GetAllAsync();
            var cfdDto = _mapper.Map<IList<Cfd>>(cfd);

            var portfolioPageDto = from p in portfolioDto
                                   join m in marketDto on p.Market_id equals m.Id into table1
                                   from m in table1.ToList()
                                   join c in cfdDto on p.Cfd_id equals c.Id into table2
                                   from c in table2.ToList()
                                   where p.State == 1
                                   select new PortfolioHistoryViewModel
                                   {
                                       Id = p.Id,
                                       Action = p.Action,
                                       Position = c.Name,
                                       Name = m.Name,
                                       Symbol = m.Symbol,
                                       Invested = p.Invested,
                                       Units = p.Units,
                                       ValueOpen = p.ValueOpen,
                                       CreatedDate = p.CreatedDate,
                                       ValueClosed = p.ValueClosed ,
                                       UpdatedDate = p.UpdatedDate,

                                       Profit = p.Cfd_id != 2 ? (m.Buying - p.ValueOpen) * (p.Invested / p.ValueOpen) : (p.ValueOpen - m.Selling) * (p.Invested / p.ValueOpen),
                                   };

            return View(portfolioPageDto);
        }



        // GET: Create
        public IActionResult Buy()
        {
            var position = JsonConvert.DeserializeObject<PortfolioPositionViewModel>((string)TempData["position"]);
            return View(position);
        }

    
        //POST /Portfolio/Create
        //[Authorize(Policy = "portfolio-create")]
        //[HttpPost("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy([Bind("Market_Id, Name, Symbol, ValueOpen, Cfd_id, Invested, StopLoss, TakeProfit, BrokerMargin")] PortfolioPositionViewModel model)
        {
            //400
            if (!ModelState.IsValid)
                return BadRequest();

            var currentUserId = 1;

            //404
            var portfolio = _mapper.Map<Portfolio>(model);


            var dbItem = await _portfolioService.AddAsync(portfolio, currentUserId);
            if (dbItem == null)
                return NotFound();

            var sm = _mapper.Map<SubjectModel>(portfolio);
            //set symbol of positionPortfolio model
            sm.Symbol = model.Symbol;
            //attach positionPortfolio to observe the selling&buying values of its asset market
            _subject.Attach(new ConcreteObserver(_subject, sm));

            //create operation debt/credit
            //create object model
            var o = new Operation
            {
                //set properties
                Action = "Open",
                Amount = Math.Round(portfolio.Invested, 2, MidpointRounding.AwayFromZero),
                Timestamp = DateTime.UtcNow,
                Portfolio_id = portfolio.Id,
               
                //1=debt, 2=credit
                OperationType_id = 1            
            };
            //create transaction on db
            var create = await _operationService.AddAsync(o, currentUserId);

            return RedirectToAction(nameof(ReadPortfolio));
        }



        // GET: Create
        public IActionResult Sell()
        {
            var position = JsonConvert.DeserializeObject<PortfolioPositionViewModel>((string)TempData["position"]);
            return View(position);
        }


        //POST /Portfolio/Create
        //[Authorize(Policy = "portfolio-create")]
        //[HttpPost("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell([Bind("Market_Id, Name, Symbol, ValueOpen, Cfd_id, Invested, StopLoss, TakeProfit, BrokerMargin")] PortfolioPositionViewModel model)
        {
            //400
            if (!ModelState.IsValid)
                return BadRequest();

            var currentUserId = 1;

            //404
            var portfolio = _mapper.Map<Portfolio>(model);
            var dbItem = await _portfolioService.AddAsync(portfolio, currentUserId);
            if (dbItem == null)
                return NotFound();

            var sm = _mapper.Map<SubjectModel>(portfolio);
            //set symbol of positionPortfolio model
            sm.Symbol = model.Symbol;
            //attach positionPortfolio to observe the selling&buying values of its asset market
            _subject.Attach(new ConcreteObserver(_subject, sm));

            //create operation debt/credit
            //create object model
            var o = new Operation
            {
                //set properties
                Action = "Open",
                Amount = Math.Round(portfolio.Invested, 2, MidpointRounding.AwayFromZero),
                Timestamp = DateTime.UtcNow,
                Portfolio_id = portfolio.Id,

                //1=debt, 2=credit
                OperationType_id = 1
            };
            //create transaction on db
            var create = await _operationService.AddAsync(o, currentUserId);

            return RedirectToAction(nameof(ReadPortfolio));
        }


        //POST /Portfolio/Update

        //[Authorize(Policy = "portfolio-update")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            var action = "Close";
            var p = await _portfolioService.FindAsync(j => j.Id == id);
            //404
            if (p == null)
                return NotFound();

            var m = await _marketService.FindAsync(j => j.Id == p.Market_id);
            //404
            if (p == null)
                return NotFound();


            //update portfolio P&L and state
            //update properties
            p.Action = action;
            p.State = 1;
            p.ValueClosed = p.Cfd_id != 2 ? m.Buying : m.Selling;
                //Math.Round(m.Buying * (1 + BrokerMP), 2, MidpointRounding.AwayFromZero);
            p.Profit = p.Cfd_id != 2 ? (m.Buying - p.ValueOpen) * (p.Invested / p.ValueOpen) : (p.ValueOpen - m.Selling) * (p.Invested / p.ValueOpen);
            p.UpdatedDate = DateTime.UtcNow;
            //update portfolio
            var update = _portfolioService.UpdateAsync(p, 1);
            if (update.Result == null)
                return NotFound();

            ////detach observer
            //var sm = _mapper.Map<SubjectModel>(p);
            ////set symbol of positionPortfolio model
            ////sm.Symbol = model.Symbol;
            ////attach positionPortfolio to observe the selling&buying values of its asset market


            //notify observers of new market asset
            //
            //get list of CFD to close
            var observers = _subject.GetObserversList();


            Console.WriteLine("number observers: {0}", observers.Count);
            if (observers.Count > 0)
            {
                //SubjectModel obs = _obs;
                foreach (Observer om in observers)
                {
                    if (om.GetObserver().Id == p.Id)
                    {
                        Console.WriteLine("manual close detach id: {0} ", om);
                        _subject.Detach(om);
                        break;
                    }//if
                }//foreach

            }//if 


            //create operation debt/credit
            //create object model
            var o = new Operation();
            //set properties
            o.Action = action;
            o.Amount = Math.Round(p.Profit, 2, MidpointRounding.AwayFromZero);
            o.Timestamp = DateTime.UtcNow;
            o.Portfolio_id = p.Id;
            //1=debt, 2=credit
            o.OperationType_id = (p.Profit + p.Invested) < 0 ? 1 : 2;
            //create transaction on db
            var create = await _operationService.AddAsync(o, 1);

            Console.WriteLine("Invoke close Observer {0}'s for asset {1} taken by {2} action",
                p.Id, m.Symbol, p.Action);


            return RedirectToAction(nameof(ReadHistory));
        }


    }
}




