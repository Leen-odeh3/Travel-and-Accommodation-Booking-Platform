using HotelBookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Data;
public class AppDbContext : IdentityDbContext<LocalUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
    {
        
    }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public DbSet<RoomClass> RoomClasses { get; set; }
   //public DbSet<IdentityUser> Users { get; set; }
  // public DbSet<IdentityRole> Roles { get; set; }
}
