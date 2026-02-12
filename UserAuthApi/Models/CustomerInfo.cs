using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Customer_Info")] // Matches the SQL table name exactly
    public class CustomerInfo
    {
        [Key]
        [Column("Customer_id")]
        public string CustomerId { get; set; } = string.Empty;

        [Column("c_first_name")]
        public string CFirstName { get; set; } = string.Empty;

        [Column("c_middle_name")]
        public string CMiddleName { get; set; } = string.Empty;

        [Column("c_last_name")]
        public string CLastName { get; set; } = string.Empty;

        [Column("c_mail_id")]
        public string CMailId { get; set; } = string.Empty;

        [Column("c_contact_info")]
        public long CContactInfo { get; set; } // bigint maps to long in C#

        [Column("c_password")]
        public string CPassword { get; set; } = string.Empty;

        [Column("c_address")]
        public string CAddress { get; set; } = string.Empty;

        [Column("c_username")]
        public string CUsername { get; set; } = string.Empty;

        [Column("vehicle_model_year")]
        public string VehicleModelYear { get; set; } = string.Empty;

        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }

        [Column("loyalty_points")]
        public int LoyaltyPoints { get; set; } = 0;

        // NEW FIELD: Maps the form's dealer_id to the DB's added_by_dealer
        [Column("added_by_dealer")]
        public string? AddedByDealer { get; set; }
    }
}