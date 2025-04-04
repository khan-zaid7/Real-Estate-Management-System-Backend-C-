using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Models;


namespace RealEstateManagement.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        // Add the DbSet for Properties
        public DbSet<Property> Properties { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
    }
}
