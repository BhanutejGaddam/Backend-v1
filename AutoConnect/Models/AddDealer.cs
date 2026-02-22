using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Dealer_info")]
    public class AddDealer
    {
        [Key]
        [Column("Dealer_id")]
        public string DealerId { get; set; } = string.Empty;

        [Column("d_first_name")]
        public string DFirstName { get; set; } = string.Empty;

        [Column("d_middle_name")]
        public string DMiddleName { get; set; } = string.Empty;

        [Column("d_last_name")]
        public string DLastName { get; set; } = string.Empty;

        [Column("d_mail_id")]
        public string DMailId { get; set; } = string.Empty;

        [Column("d_contact_info")]
        public long DContactInfo { get; set; } // bigint maps to long in C#

        [Column("Store_name")]
        public string StoreName { get; set; } = string.Empty;

        [Column("d_password")]
        public string DPassword { get; set; } = string.Empty;

        [Column("Store_address")]
        public string StoreAddress { get; set; } = string.Empty;

        [Column("city")]
        public string City { get; set; } = string.Empty;

        [Column("state")]
        public string State { get; set; } = string.Empty;

        [Column("d_username")]
        public string DUsername { get; set; } = string.Empty;
    }
}
