using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using esstp.Models.ViewModels;
using Microsoft.Extensions.Hosting;

namespace esstp.Models.Services
{
    
    public class ProducerCatalogueService : IHostedService, IDisposable//: BackgroundService
    {
        //PRODUCER
       
        private Queue _queue;
        private Timer _timer;

        public ProducerCatalogueService(Queue queue)//MarketService marketService)//, MarketController marketController)
        {
            //_marketController = marketController;
            //_marketService = marketService;
            _queue = queue;
        }
        //When deriving from the BackgroundService abstract base class,
        //thanks to that inherited implementation, we just need to implement the ExecuteAsync() method
        //in own custom hosted service class
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    Console.WriteLine($"CatalogueNotifications is starting.");

        //    //registes cancellation token handler
        //    stoppingToken.Register(() =>
        //        Console.WriteLine($"CatalogueNotifications background task is stopping."));

        //    //long running task
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        //timestamp
        //        var datetime = DateTime.UtcNow;
        //        string iso8601 = datetime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
        //        Console.WriteLine($"CatalogueNotifications task doing background work at " + iso8601);

        //        //new event
        //        Market mkt = new Market();

        //        //push event to queue
        //        _queue.Push(mkt);

        //        Console.WriteLine("Push " + mkt.Symbol + ", queue.size: " + _queue.Size());


        //        //get pooling time setting from appsettings.json file
        //        var PoolingTimeInSeconds = AppConfig.Config["AppSettings:PoolingTimeInSeconds"];

        //        //wait pooling time until produce another event
        //        await Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(PoolingTimeInSeconds)), stoppingToken);
        //    }

        //    Console.WriteLine($"CatalogueNotifications background task is stopping.");
        //}

        public Task StartAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"ProducerCatalogue is starting.");
            var MaxPushTimeInSeconds = AppConfig.Config["AppSettings:MaxPushTimeInSeconds"];
           
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(Convert.ToDouble(MaxPushTimeInSeconds)));

            return Task.CompletedTask;
        }



        private void DoWork(object state)
        {
            //timestamp
            var datetime = DateTime.UtcNow;
            string iso8601 = datetime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            Console.WriteLine($"ProducerCatalogue task doing background work at " + iso8601);

            //new event
            MarketViewModel mkt = new MarketViewModel();

            //push event to queue
            _queue.Push(mkt);

            Console.WriteLine("Push " + mkt.Symbol + ", queue.size: " + _queue.Size());


        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }



    }
}
