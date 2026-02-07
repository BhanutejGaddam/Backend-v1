using Microsoft.EntityFrameworkCore;
using UserAuthApi.Models;

namespace UserAuthApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Make sure this says <CustomerInfo> and NOT <User>
        public DbSet<CustomerInfo> Customers { get; set; }
    }
}

