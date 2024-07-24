using HotelBookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Data;
public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
    {
        
    }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }
}
