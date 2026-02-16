using Microsoft.EntityFrameworkCore;
using UserAuthApi.Models;

namespace UserAuthApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Make sure this says <CustomerInfo> and NOT <User>
        public DbSet<CustomerInfo> Customers { get; set; }
        public DbSet<AdminInfo> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<WarrantyCompliance> WarrantyCompliances { get; set; }

        public DbSet<ComplianceInformation> ComplianceInformations { get; set; }
        public DbSet<AddDealer> Dealers { get; set; }
        public DbSet<VehicleInventory> VehicleInventories { get; set; }
        public DbSet<SparePartInventory> SparePartInventories { get; set; }

        public DbSet<VehicleSalesInfo> VehicleSalesInfo { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for VehicleSalesInfo
            modelBuilder.Entity<VehicleSalesInfo>()
                .HasKey(v => new { v.DealerId, v.CustomerId, v.SoldDate });
        }
    }
}




