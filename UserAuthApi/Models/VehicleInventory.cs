using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Vehile_Inventory_info")]
    public class VehicleInventory
    {
        [Column("Dealer_id")]
        public string DealerId { get; set; } = string.Empty;

        [Key]
        [Column("Vehicle_id")]
        public string VehicleId { get; set; } = string.Empty;

        [Column("availability_status")]
        public bool IsAvailable { get; set; }

        [Column("Model_info")]
        public string ModelInfo { get; set; } = string.Empty;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("available_units")]
        public int? AvailableUnits { get; set; } // Matches [int] NULL
    }
}