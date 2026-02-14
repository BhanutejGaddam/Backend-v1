using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Vehicle_sales_info")]
    public class VehicleSalesInfo
    {
        [Column("Dealer_id", Order = 0)]
        public string DealerId { get; set; } = string.Empty;

        [Column("Customer_id", Order = 1)]
        public string CustomerId { get; set; } = string.Empty;

        [Column("Vehicle_sold")]
        [StringLength(100)]
        public string VehicleSold { get; set; } = string.Empty;

        
        [Column("sold_date", Order = 2)]
        public DateTime SoldDate { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        // Navigation properties (optional but recommended for EF Core)
        [ForeignKey("CustomerId")]
        public virtual CustomerInfo? Customer { get; set; }

        [ForeignKey("DealerId")]
        public virtual AddDealer? Dealer { get; set; }
    }
}