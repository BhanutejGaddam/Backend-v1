using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Dealer_info")]
    public class DealerInfo
    {
        [Key]
        [Column("Dealer_id")]
        public string DealerId { get; set; } = string.Empty;

        // CHANGE THE NAMES INSIDE [Column("...")] TO MATCH SSMS EXACTLY
        [Column("d_mail_id")]
        public string DMailId { get; set; } = string.Empty;

        [Column("d_password")]
        public string DPassword { get; set; } = string.Empty;
    }
}