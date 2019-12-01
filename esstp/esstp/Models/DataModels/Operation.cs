using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace esstp.Models
{
    [Table("Operation")]
    public class Operation 
    {

        [Key]
        public int Id { get; set; }
        public string Action { get; set; }
        public double  Amount { get; set; }
        public DateTime  Timestamp { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public short? IsInactive { get; set; }
        public int? OperationType_id { get; set; }
        public int? Portfolio_id { get; set; }



    
    }
}


