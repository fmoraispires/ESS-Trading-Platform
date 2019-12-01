using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace esstp.Models
{
    [Table("Permission")]
    public class Permission
    {

        [Key]
        public int Oid { get; set; }
        public string PermissionName { get; set; }
        public string PermissionValue { get; set; }
        public string PermissionType { get; set; }
        public string Discriminator { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public short? IsInactive { get; set; }
        public int? Role_Oid { get; set; }
    }
}
