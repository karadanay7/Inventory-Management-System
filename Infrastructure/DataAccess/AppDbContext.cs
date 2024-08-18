using Application.Extension.Identity;
using Domain.Entities.ActivityTracker;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Infrastructure.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
       
        }
        public DbSet<Tracker> ActivityTracker { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                // Check if the database can be connected
                var canConnect = await this.Database.CanConnectAsync();
                Console.WriteLine($"Database connection test result: {(canConnect ? "Connected" : "Not Connected")}");

                // Optionally, perform a simple query to further verify
                if (canConnect)
                {
                    var userCount = await this.Users.CountAsync();
                    Console.WriteLine($"Number of users in the database: {userCount}");
                }

                return canConnect;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during database connection test: {ex.Message}");
                return false;
            }
        }
    }
}
