using System;
using System.ComponentModel.DataAnnotations;

namespace esstp.Models.ViewModels
{
    public class PortfolioPageViewModel
    {
        public int Id { get; set; } //portfolio
        public string Position { get; set; } //portfolio
        public string Name { get; set; } //market
        public string Symbol { get; set; } //market
        public double Units { get; set; } //market
        public double ValueOpen { get; set; } //portfolio
        [DisplayFormat(DataFormatString = "{0:N2}")]
        //[Display(Name = "EUR")]
        public double Invested { get; set; } //portfolio
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double CurrentProfit { get; set; } // portfolio.valuopen / market.current
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double CurrentInvested { get; set; } //portfolio.invested * currentprofit
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Current { get; set; } //market.current


    }
}
