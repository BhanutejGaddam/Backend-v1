using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Admin_info")] // Maps this class to your Admin table in SSMS
    public class AdminInfo
    {
        [Key]
        [Column("Admin_id")] // Maps property to your SQL Column name
        public string AdminId { get; set; } = string.Empty;

        [Column("A_mail_id")]
        public string AMailId { get; set; } = string.Empty;

        [Column("A_password")]
        public string APassword { get; set; } = string.Empty;
    }
}