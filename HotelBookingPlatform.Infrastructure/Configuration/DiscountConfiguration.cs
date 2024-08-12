namespace HotelBookingPlatform.Infrastructure.Configuration;
public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.Property(d => d.Percentage)
            .HasColumnType("DECIMAL(5,2)");
    }
}
