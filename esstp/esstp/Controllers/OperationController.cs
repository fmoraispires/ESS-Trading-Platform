using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using esstp.Models.Services;
using esstp.Models.ViewModels;
using esstp.Models;
using System.Linq;

namespace esstp.Controllers
{

    //[Authorize]
    //[Produces("application/json")]
    //[ApiController]
    //[Route("api/[controller]")]
    public class OperationController : Controller
    {
        private IOperationService _operationService;
        private IOperationTypeService _operationtypeService;
        private IPortfolioService _portfolioService;
        private IMarketService _marketService;
        private ICfdService _cfdService;
        private IMapper _mapper;

        public OperationController(
                IOperationService operationService,
                IOperationTypeService operationtypeService,
                IPortfolioService portfolioService,
                IMarketService marketService,
                ICfdService cfdService,
                IMapper mapper
            )
        {
            _operationService = operationService;
            _operationtypeService = operationtypeService;
            _portfolioService = portfolioService;
            _marketService = marketService;
            _cfdService = cfdService;
            _mapper = mapper;
        }



        // GET /operation/ReadOperation

        //ReadOperation
        //[Authorize(Policy = "operation-readall")]
        //[HttpGet("ReadOperation")]
        public async Task<IActionResult> ReadOperation()
        {
            var operation = await _operationService.GetAllAsync();
            var operationDto = _mapper.Map<IList<Operation>>(operation);
            var opertype = await _operationtypeService.GetAllAsync();
            var opertypeDto = _mapper.Map<IList<OperationType>>(opertype);
            var portfolio = await _portfolioService.GetAllAsync();
            var portfolioDto = _mapper.Map<IList<Portfolio>>(portfolio);
            var market = await _marketService.GetAllAsync();
            var marketDto = _mapper.Map<IList<Market>>(market);
            var cfd = await _cfdService.GetAllAsync();
            var cfdDto = _mapper.Map<IList<Cfd>>(cfd);

            var operationPageDto = from o in operationDto
                                   join ot in opertypeDto on o.OperationType_id  equals ot.Id into table1
                                   from ot in table1.ToList()
                                   join p in portfolioDto on o.Portfolio_id equals p.Id into table2
                                   from p in table2.ToList()
                                   join m in marketDto on p.Market_id equals m.Id into table3
                                   from m in table3.ToList()
                                   join c in cfdDto on p.Cfd_id equals c.Id into table4
                                   from c in table4.ToList()

                                   where o.CreatedBy == 1
                                   select new OperationPageViewModel
                                   {
                                       Id = o.Id,
                                       Amount = o.Amount,
                                       Timestamp = o.Timestamp,
                                       OperationType = ot.Name,
                                       Action = o.Action,
                                       Position = c.Name,
                                       MarketName = m.Name,
                                       Symbol = m.Symbol
                                   };

            return View(operationPageDto);
        }







    }

}