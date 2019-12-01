using System;
namespace esstp.Models.ViewModels
{
    public class OperationPageViewModel
    {

        public int Id { get; set; } //operation
        public string Action { get; set; } //operation
        public double Amount { get; set; } //operation
        public DateTime Timestamp { get; set; } //operation
        public string OperationType { get; set; } //operationType
        public string Position { get; set; } //portfolio
        public string MarketName { get; set; } //market
        public string Symbol { get; set; } //market

    }
}
