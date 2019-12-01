using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace esstp.Models
{
    [Table("Market")]
    public class Market
    {

        [Key]
        public int Id { get; set; }
        public int Isin { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Buying { get; set; }
        public double Selling { get; set; }
        public short? IsInactive { get; set; }
        public int? Asset_id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public static explicit operator Market(string v)
        {
            throw new NotImplementedException();
        }
    }
}

