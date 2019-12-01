using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Linq;
using esstp.Models.ViewModels;

namespace esstp.Models.Services
{
    public class ConsumerMarketService : IHostedService, IDisposable
    {
        private Queue _queue;
        private Timer _timer;
        private IMapper _mapper;
        private ConcreteSubject _subject;



        public ConsumerMarketService(
            IServiceProvider services,
            Queue queue,
            IMapper mapper,
            ConcreteSubject subject)
        {
            _queue = queue;
            Services = services;
            _mapper = mapper;
            _subject = subject;
        }


        
        public IServiceProvider Services { get; }
        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"ConsumerMarketService is starting.");


            //save on database
            using (var scope = Services.CreateScope())
            {
               
                //create scoped MarketService to access Market datamodel on database
                var scopedPortfolioService =
                        scope.ServiceProvider
                            .GetRequiredService<IPortfolioService>();

                //get existing portfolio that match pulled market.Isin
                // and have its CFD open
                //var dbPItem = scopedPortfolioService.GetAllAsync();
                var dbPItem = scopedPortfolioService.FindAllAsync(s => s.State == 0);

                //create scoped MarketService to access Market datamodel on database
                var scopedMarketService =
                        scope.ServiceProvider
                            .GetRequiredService<IMarketService>();

                //get existing db data market that match pulled market.Isin 
                var dbMItem = scopedMarketService.GetAllAsync();

                //Attach Observers ( attach all existing CFD portfolio in open state)
                foreach (Portfolio p in dbPItem.Result)
                {
                    //map portfolio model to positionPortfolio model
                    var o = _mapper.Map<SubjectModel>(p);
                    //filter the market containing the pulled market asset
                    var x = dbMItem.Result.First(s => s.Id == o.Market_Id);
                    //set symbol of positionPortfolio model
                    o.Symbol = x.Symbol ;
                    //attach positionPortfolio to observe the selling&buying values of its asset market
                    _subject.Attach(new ConcreteObserver(_subject, o));
                }
            }

            var ProcessEventsMaxRateInSeconds = AppConfig.Config["AppSettings:ProcessEventsMaxRateInSeconds"];

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(Convert.ToDouble(ProcessEventsMaxRateInSeconds)));
            return Task.CompletedTask;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            
            var datetime = DateTime.UtcNow;
            string iso8601 = datetime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            Console.WriteLine($"ConsumerMarketService task doing background work at " + iso8601);

            //pull event from the queue
            MarketViewModel mkt = _queue.Pull();

            
            Console.WriteLine("Pull " + mkt.Symbol + ", queue.size: " + _queue.Size());


            //save on database
            using (var scope = Services.CreateScope())
            {
                string saveResult = "failed.";

                //create scoped MarketService to access Market datamodel on database
                var scopedProcessingService =
                        scope.ServiceProvider
                            .GetRequiredService<IMarketService>();

                //get existing db data market that match pulled market.Isin 
                var dbItem = scopedProcessingService.GetAsync(mkt.Isin); 

                //if exists on db then updates record
                if (dbItem.Result != null)
                    {
                    var _id = dbItem.Result.Id;
                    // map dto to entity

                    var m = mkt.GetDbItem(dbItem.Result);
                    
                    //update object
                    m.Id = _id;
                    m.CreatedBy = dbItem.Result.CreatedBy;
                    m.CreatedDate = dbItem.Result.CreatedDate;

 
                    var update = scopedProcessingService.UpdateAsync(m, 1);
                    if (update.Result != null)
                        saveResult = "record updated.";

                }
                //if not exists on db then creates record
                else
                    {
                    var m = _mapper.Map<Market>(mkt);

                    var create = scopedProcessingService.AddAsync(m, 1);
                    if (create.Result != null)
                        saveResult = "record created.";

                }

                Console.WriteLine("Save " + mkt.Symbol + " on database using IMarketService: " + saveResult);

                //map market model to positionPortfolio model
                var pp = _mapper.Map<SubjectModel>(mkt);
                pp.Symbol = mkt.Symbol;
                pp.Buying = mkt.Buying;
                pp.Selling = mkt.Selling;

                //set concreteSubject to the new arrived asset market contained in the positionPortfolio model 
                _subject.SubjectState = pp;

               
                //notify observers of new market asset
                //
                //get list of CFD to close
                var numberobservers = _subject.GetObserversList().Count;
                Console.WriteLine("number observers: {0}", numberobservers);
                if (numberobservers > 0)
                {
                    var notifyResult = _subject.Notify();

                    Console.WriteLine("number observers to update: {0}", notifyResult.Count);
                    //if the list contain CFDs
                    if (notifyResult.Count > 0)
                    {   
                        //foreach CFD calculate P&L and close
                        foreach (ObserverModel om in notifyResult)
                        {
                            Console.WriteLine("detach id: {0}, action: {1} ", om.Id, om.Action);
                            _subject.Detach(om.O);
                            Close(om.Id, om.Action).GetAwaiter();
       
                        }
                    }//if

                }//if 



            }//using


        }




        //calculate P&L and close CFDs in Portfolio
        //save transaction with debt/credit amount to the user
        public async Task Close(int id, string action)
        {
            //Console.WriteLine("START Invoke close Observer {0}'s for {1} action",
            //           id, action);

            try
            {
                //update database
                using (var scope = Services.CreateScope())
                {
                    
                    //create scoped PortfolioService to access Portfolio datamodel on database
                    var _portfolioService =
                            scope.ServiceProvider
                                .GetRequiredService<IPortfolioService>();

                    //create scoped MarketService to access Market datamodel on database
                    var _marketService =
                            scope.ServiceProvider
                                .GetRequiredService<IMarketService>();

                    //create scoped OperationService to access Operation datamodel on database
                    var _operationService =
                            scope.ServiceProvider
                                .GetRequiredService<IOperationService>();


                    var p = await _portfolioService.FindAsync(j => j.Id == id);
                    
                    var m = await _marketService.FindAsync(j => j.Id == p.Market_id);
                    
                    //update portfolio P&L and state
                    //update properties
                    p.Action = action;
                    p.State = 1;
                    p.ValueClosed = p.Cfd_id != 2 ? m.Buying : m.Selling;
                    p.Profit = p.Cfd_id != 2 ? (m.Buying - p.ValueOpen) * (p.Invested / p.ValueOpen) : (p.ValueOpen - m.Selling) * (p.Invested / p.ValueOpen);
                    p.UpdatedDate = DateTime.UtcNow;
                    //update portfolio
                    var update = await _portfolioService.UpdateAsync(p, 1);

                    //create operation debt/credit
                    //create object model
                    var o = new Operation
                    {
                        //set properties
                        Action = action,
                        Amount = Math.Round(p.Profit + p.Invested, 2, MidpointRounding.AwayFromZero),
                        Timestamp = DateTime.UtcNow,
                        Portfolio_id = p.Id,
                        //1=debt, 2=credit
                        OperationType_id = (p.Profit + p.Invested) < 0 ? 1 : 2
                    };
                    //create transaction on db
                    var create = await _operationService.AddAsync(o, 1);
                   

                    //Console.WriteLine("Invoke close Observer {0}'s for asset {1} taken by {2} action",
                    //    p.Id, m.Symbol, p.Action);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Close exception: " + e);
               
            }
            var datetime = DateTime.UtcNow;
            string iso8601 = datetime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            Console.WriteLine("Invoke close Observer {0}'s for {1} action, at {2}",
                        id, action, iso8601);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }


        private async Task<double> GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return cpuUsageTotal * 100;
        }

    }
}
