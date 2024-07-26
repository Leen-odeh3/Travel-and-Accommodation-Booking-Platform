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
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
    }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Owner> owners { get; set; }
    public DbSet<City> cities { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<LocalUser> LocalUsers { get; set; }
    public DbSet<RoomClass> RoomClasses { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<InvoiceRecord> InvoiceRecords { get; set; }


}
