using HotelBookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Configuration;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(b => b.TotalPrice)
            .HasColumnType("DECIMAL(18,2)");

        builder.HasOne(b => b.LocalUser)
             .WithMany(u => u.Bookings)
             .HasForeignKey(b => b.LocalUserId);
    }
}
