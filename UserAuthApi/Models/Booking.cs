using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthApi.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public long BookingId { get; set; } // Identity column

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

        [Column("Address")]
        public string? Address { get; set; }

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
        public bool Pickup_Dropoff { get; set; }
        public bool Availed_Warranty { get; set; }

        // Individual Booleans for Warranty Checkboxes
        public bool Engine_Check { get; set; }
        public bool Brake_Inspection { get; set; }
        public bool Oil_Change { get; set; }
        public bool Transmission_Service { get; set; }
        public bool Battery_Replacement { get; set; }
        public bool Tire_Rotation { get; set; }
        public bool Suspension_Check { get; set; }
        public bool Electrical_System { get; set; }
        public bool Cooling_System { get; set; }
        public bool Exhaust_System { get; set; }

        public string? Selected_Dealer { get; set; }
        public string? Selected_Dealer_Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }

        [Column("booking_status")] // This tells EF to look for the lowercase name in SQL
        public string BookingStatus { get; set; } = "BOOKED";

        [Column("total_bill")]
        public decimal? TotalBill { get; set; } // Matches DECIMAL(10,2)
    }
}