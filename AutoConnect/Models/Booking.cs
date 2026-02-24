using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public long BookingId { get; set; }

        [Column("Customer_id")]
        public string CustomerId { get; set; } = string.Empty;

        [Column("Full_Name")]
        public string FullName { get; set; } = string.Empty;

        [Column("Contact_Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Column("Emergency_Contact")]
        public string? EmergencyContact { get; set; }

        [Column("Email_Address")]
        public string? EmailAddress { get; set; }

        public string? Address { get; set; }

        // CHANGE THIS TO STRING
        [Column("Vehicle_Model_Year")]
        public string? VehicleModelYear { get; set; }

        [Column("VIN_Chassis_Number")]
        public string? VinChassisNumber { get; set; }

        [Column("Registration_Number")]
        public string? RegistrationNumber { get; set; }

        [Column("Current_Mileage")]
        public int? CurrentMileage { get; set; }

        [Column("Fuel_Type")]
        public string? FuelType { get; set; }

        [Column("Type_of_Service")]
        public string? TypeOfService { get; set; }

        [Column("Description_of_Issues")]
        public string? DescriptionOfIssues { get; set; }

        [Column("Preferred_Service_Package")]
        public string? PreferredServicePackage { get; set; }

        [Column("Previous_Service_History")]
        public string? PreviousServiceHistory { get; set; }

        public DateTime Slot { get; set; }

        [Column("Pickup_Dropoff")]
        public bool Pickup_Dropoff { get; set; }

        [Column("Availed_Warranty")]
        public bool Availed_Warranty { get; set; }

        // Warranty Booleans
        [Column("Engine_Check")] public bool Engine_Check { get; set; }
        [Column("Brake_Inspection")] public bool Brake_Inspection { get; set; }
        [Column("Oil_Change")] public bool Oil_Change { get; set; }
        [Column("Transmission_Service")] public bool Transmission_Service { get; set; }
        [Column("Battery_Replacement")] public bool Battery_Replacement { get; set; }
        [Column("Tire_Rotation")] public bool Tire_Rotation { get; set; }
        [Column("Suspension_Check")] public bool Suspension_Check { get; set; }
        [Column("Electrical_System")] public bool Electrical_System { get; set; }
        [Column("Cooling_System")] public bool Cooling_System { get; set; }
        [Column("Exhaust_System")] public bool Exhaust_System { get; set; }

        [Column("Selected_Dealer_Id")]
        public string? Selected_Dealer_Id { get; set; }


        [Column("Selected_Dealer")]
        public string? Selected_Dealer { get; set; }

        [Column("total_bill")]
        public decimal? TotalBill { get; set; }


        [Column("booking_status")]
        public string BookingStatus { get; set; } = "BOOKED";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    }
}
