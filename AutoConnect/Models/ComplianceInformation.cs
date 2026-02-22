using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("compliance_information")]
    public class ComplianceInformation
    {
        [Key]
        [Column("vehicle_number")]
        public string VehicleNumber { get; set; } = string.Empty;

        [Column("pollution_check")]
        public string PollutionCheck { get; set; } = string.Empty;

        [Column("fitness_check")]
        public string FitnessCheck { get; set; } = string.Empty;

        [Column("rc_check")]
        public string RcCheck { get; set; } = string.Empty;

        [Column("last_checked")]
        public DateTime LastChecked { get; set; }

        [Column("expiry")]
        public DateTime Expiry { get; set; }

        [Column("dealer_id")]
        public string dealerId { get; set; } = string.Empty;

        [Column("customer_id")]
        public string customer_id { get; set; } = string.Empty;
    }
}