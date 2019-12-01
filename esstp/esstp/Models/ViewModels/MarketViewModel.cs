using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using esstp.Models.Services;

namespace esstp.Models.ViewModels
{
    public class MarketViewModel
    {

        public int Id { get; set; }
        public int Isin { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Buying { get; set; }
        public double Selling { get; set; }
        public string Asset { get; set; }
        public int? Asset_id { get; set; }

        public Market GetDbItem(Market dbItem)
        {
            dbItem.Id = Id;
            dbItem.Isin = Isin;
            dbItem.Name = Name;
            dbItem.Symbol = Symbol;
            dbItem.Buying = Buying;
            dbItem.Selling = Selling;
            dbItem.Asset_id = Asset_id;
            return dbItem;
        }

        public MarketViewModel()
        {
            //2018-12-10T17:31:16+00:00
            //Int32 unixTimestamp = (Int32)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            var rnd = new Random();
            Dictionary<int, dynamic> market = new Dictionary<int, dynamic>();

            market[0] = (new { Isin = "1", Name = "Gold", Symbol = "GOLD", Buying = 1457.0, Selling = 1458.0, Asset_id = 1 });
            market[1] = (new { Isin = "2", Name = "Oil", Symbol = "OIL", Buying = 57.0, Selling = 58.0, Asset_id = 1 });
            market[2] = (new { Isin = "3", Name = "Silver", Symbol = "SILVER", Buying = 16.0, Selling = 16.0, Asset_id = 1 });
            market[3] = (new { Isin = "4", Name = "Google", Symbol = "GOOG", Buying = 1308.0, Selling = 1310.0, Asset_id = 2 });
            market[4] = (new { Isin = "5", Name = "Apple", Symbol = "AAPL", Buying = 259.0, Selling = 260.0, Asset_id = 2 });
            market[5] = (new { Isin = "6", Name = "Nvidia", Symbol = "NVDA", Buying = 207.0, Selling = 207.0, Asset_id = 2 });
            market[6] = (new { Isin = "7", Name = "Alibaba", Symbol = "BABA", Buying = 8.0, Selling = 9.0, Asset_id = 2 });
            market[7] = (new { Isin = "8", Name = "IBM", Symbol = "IBM", Buying = 137.0, Selling = 137.0, Asset_id = 2 });

            //select random event
            int index = rnd.Next(0, market.Count);

            //set object properties
            Isin = Convert.ToInt32(market[index].Isin);
            Name = market[index].Name;
            Symbol = market[index].Symbol;
            Buying = Convert.ToDouble(market[index].Buying) + Convert.ToDouble(rnd.Next(0, 99)) / 10;
            Selling = Convert.ToDouble(market[index].Buying) + Convert.ToDouble(rnd.Next(0, 99)) / 10;
            Asset_id = Convert.ToInt32(market[index].Asset_id);
        }







    }
}

