using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Data;
public class AppDbContext : IdentityDbContext<LocalUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new RoomClassConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());

    }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public DbSet<RoomClass> RoomClasses { get; set; }
    public DbSet<Room> Rooms { get; set; }
 
}
