using System;
namespace esstp.Models.ViewModels
{
    public class SubjectModel
    {

        public int Id { get; set; } //portfolio
        public short State { get; set; } //portfolio
        public double Invested { get; set; } //portfolio
        public double Units { get; set; } //portfolio
        public double ValueOpen { get; set; } // portfolio
        public double StopLoss { get; set; } //portfolio 
        public double TakeProfit { get; set; } //portfolio
        public int Cfd_Id { get; set; } //portfolio
        public int Market_Id { get; set; } //portfolio
        public double Buying { get; set; } //market
        public double Selling { get; set; } //market
        public string Symbol { get; set; } //market

    }
}
