using HotelBookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Configuration;
public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.RoomID);

        builder.Property(r => r.Number)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(r => r.CreatedAtUtc)
               .IsRequired();

        builder.Property(rc => rc.AdultsCapacity)
           .IsRequired();

        builder.Property(rc => rc.ChildrenCapacity)
            .IsRequired();

        builder.Property(rc => rc.PricePerNight)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(r => r.RoomClass)
               .WithMany(rc => rc.Rooms)
               .HasForeignKey(r => r.RoomClassID);

        builder.HasMany(rc => rc.Bookings)
             .WithMany(a => a.Rooms)
             .UsingEntity<Dictionary<string, object>>(
                 "BookingRoom",
                 rca => rca.HasOne<Booking>().WithMany().HasForeignKey("BookingID"),
                 rca => rca.HasOne<Room>().WithMany().HasForeignKey("RoomID")
             );
    }
}
