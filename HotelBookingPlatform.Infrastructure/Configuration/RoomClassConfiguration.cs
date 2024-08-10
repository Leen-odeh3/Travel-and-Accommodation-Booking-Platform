namespace HotelBookingPlatform.Infrastructure.Configuration;
public class RoomClassConfiguration : IEntityTypeConfiguration<RoomClass>
{
    public void Configure(EntityTypeBuilder<RoomClass> builder)
    {
        builder.HasKey(rc => rc.RoomClassID);

        builder.Property(rc => rc.RoomType)
            .IsRequired();

        builder.Property(rc => rc.Description)
            .HasMaxLength(500);

        builder.Property(rc => rc.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(rc => rc.Hotel)
            .WithMany(h => h.RoomClasses)
            .HasForeignKey(rc => rc.HotelId);

        builder.HasMany(rc => rc.Amenities)
               .WithMany(a => a.RoomClasses)
               .UsingEntity<Dictionary<string, object>>(
                   "RoomClassAmenity",
                   rca => rca.HasOne<Amenity>().WithMany().HasForeignKey("AmenityID"),
                   rca => rca.HasOne<RoomClass>().WithMany().HasForeignKey("RoomClassID")
               );
    }
}
