using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace esstp.Models
{
    [Table("Portfolio")]
    public class Portfolio 
    {

        [Key]
        public int Id { get; set; }
        public short State { get; set; }
        public string Action { get; set; }
        public double ValueOpen { get; set; }
        public double ValueClosed { get; set; }
        public double Invested { get; set; }
        public double StopLoss { get; set; }
        public double TakeProfit { get; set; }
        public double Units { get; set; }
        public double BrokerMargin { get; set; }
        public double Profit { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public short? IsInactive { get; set; }
        public int? Cfd_id { get; set; }
        public int? Market_id { get; set; }
        public int? User_id { get; set; }

    }
}


