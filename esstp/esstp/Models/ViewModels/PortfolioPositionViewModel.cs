using System;
using System.ComponentModel.DataAnnotations;

namespace esstp.Models.ViewModels
{
    public class PortfolioPositionViewModel
    {
        public string Name { get; set; } //market
        public string Symbol { get; set; } //market
        public string Position { get; set; } //cfd
        public DateTime? CreatedDate { get; set; } //portfolio
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Invested { get; set; } //portfolio
        public double Units { get; set; } //portfolio
        public double ValueOpen { get; set; } //portfolio
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Current { get; set; } //market //market.current
        public double StopLoss { get; set; } // portfolio
        public double TakeProfit { get; set; } //portfolio
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double CurrentProfit { get; set; } //market // portfolio.valuopen / market.current
        public int Cfd_id { get; set; } //portfolio
        public int Market_Id { get; set; } //portfolio
        public double BrokerMargin { get; set; } //portfolio
    }
}
