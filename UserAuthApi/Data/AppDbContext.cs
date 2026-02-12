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
    }
}




