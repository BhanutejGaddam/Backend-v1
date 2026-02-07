using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToAutoConnect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "Customer_info",
                columns: table => new
                {
                    Customer_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    c_first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_middle_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_mail_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_contact_info = table.Column<long>(type: "bigint", nullable: false),
                    c_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vehicle_model_year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    loyalty_points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_info", x => x.Customer_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer_info");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }
    }
}
