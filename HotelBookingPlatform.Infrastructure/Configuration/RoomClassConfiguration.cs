using HotelBookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Configuration;
public class RoomClassConfiguration : IEntityTypeConfiguration<RoomClass>
{
    public void Configure(EntityTypeBuilder<RoomClass> builder)
    {
        builder.HasKey(rc => rc.RoomClassID);

        builder.Property(rc => rc.RoomType)
            .IsRequired();

        builder.Property(rc => rc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rc => rc.Description)
            .HasMaxLength(500);

        builder.Property(rc => rc.AdultsCapacity)
            .IsRequired();

        builder.Property(rc => rc.ChildrenCapacity)
            .IsRequired();

        builder.Property(rc => rc.PricePerNight)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(rc => rc.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(rc => rc.Hotel)
            .WithMany(h => h.RoomClasses)
            .HasForeignKey(rc => rc.HotelId);
    }
}
