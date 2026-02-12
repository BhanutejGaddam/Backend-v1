using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("SpareParts_Inventory_info")]
    public class SparePartInventory
    {
        [Column("Dealer_id")]
        public string DealerId { get; set; } = string.Empty;

        [Key]
        [Column("SparePart_id")]
        public string SparePartId { get; set; } = string.Empty;

        [Column("SparePart_name")]
        public string SparePartName { get; set; } = string.Empty;

        [Column("availability_status")]
        public bool IsAvailable { get; set; }

        [Column("Sparepart_price")]
        public decimal SparePartPrice { get; set; }

        [Column("available_units")]
        public int? AvailableUnits { get; set; } // Matches [int] NULL
    }
}