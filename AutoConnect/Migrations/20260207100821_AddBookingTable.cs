using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin_info",
                columns: table => new
                {
                    Admin_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    A_mail_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A_password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin_info", x => x.Admin_id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Full_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emergency_Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vehicle_Model_Year = table.Column<short>(type: "smallint", nullable: true),
                    VIN_Chassis_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Registration_Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Current_Mileage = table.Column<int>(type: "int", nullable: true),
                    Fuel_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type_of_Service = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description_of_Issues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preferred_Service_Package = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Previous_Service_History = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pickup_Dropoff = table.Column<bool>(type: "bit", nullable: false),
                    Availed_Warranty = table.Column<bool>(type: "bit", nullable: false),
                    Engine_Check = table.Column<bool>(type: "bit", nullable: false),
                    Brake_Inspection = table.Column<bool>(type: "bit", nullable: false),
                    Oil_Change = table.Column<bool>(type: "bit", nullable: false),
                    Transmission_Service = table.Column<bool>(type: "bit", nullable: false),
                    Battery_Replacement = table.Column<bool>(type: "bit", nullable: false),
                    Tire_Rotation = table.Column<bool>(type: "bit", nullable: false),
                    Suspension_Check = table.Column<bool>(type: "bit", nullable: false),
                    Electrical_System = table.Column<bool>(type: "bit", nullable: false),
                    Cooling_System = table.Column<bool>(type: "bit", nullable: false),
                    Exhaust_System = table.Column<bool>(type: "bit", nullable: false),
                    Selected_Dealer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selected_Dealer_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingId);
                });

            migrationBuilder.CreateTable(
                name: "Dealer_info",
                columns: table => new
                {
                    Dealer_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    d_mail_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    d_password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealer_info", x => x.Dealer_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin_info");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Dealer_info");
        }
    }
}
