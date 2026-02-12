using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Warranty_compliance_info")]
    public class WarrantyCompliance
    {
        [Key]
        [Column("Vehicle_Number")]
        public string VehicleNumber { get; set; } = string.Empty;

        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [Column("Issued_date")]
        public DateTime IssuedDate { get; set; }

        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }
    }
}