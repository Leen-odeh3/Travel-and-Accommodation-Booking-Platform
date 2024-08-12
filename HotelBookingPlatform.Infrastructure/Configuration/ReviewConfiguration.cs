namespace HotelBookingPlatform.Infrastructure.Configuration;
public class ReviewConfiguration: IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Hotel)
       .WithMany(h => h.Reviews)
       .HasForeignKey(r => r.HotelId)
       .OnDelete(DeleteBehavior.Restrict);

    }
}