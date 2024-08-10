namespace HotelBookingPlatform.Infrastructure.Configuration;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(b => b.TotalPrice)
            .HasColumnType("DECIMAL(18,2)");

        builder.HasOne(r => r.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}
