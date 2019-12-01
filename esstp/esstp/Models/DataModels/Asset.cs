using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace esstp.Models
{
    [Table("Asset")]
    public class Asset 
    {

        [Key]
        public int Id { get; set; }
        public string  Name { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public short? IsInactive { get; set; }

    }
}


