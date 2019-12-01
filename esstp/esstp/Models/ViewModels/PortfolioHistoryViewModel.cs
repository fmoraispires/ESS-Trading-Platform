using System;
using System.ComponentModel.DataAnnotations;

namespace esstp.Models.ViewModels
{
    public class PortfolioHistoryViewModel
    {
        public int Id { get; set; } //portfolio
        public string Action { get; set; } //portfolio
        public string Position { get; set; } //portfolio
        public string Name { get; set; } //market
        public string Symbol { get; set; } //market
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Invested { get; set; } //portfolio
        public double Units { get; set; } //portfolio
        public double ValueOpen { get; set; } //portfolio
        [Display(Name = "OpenTime")]
        public DateTime? CreatedDate { get; set; }
        public double ValueClosed { get; set; } //portfolio
        [Display(Name = "CloseTime")]
        public DateTime? UpdatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Profit { get; set; } // 
    }
}
