using HotelBookingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Configuration;
public class ReviewConfiguration: IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {

        builder.HasOne(b => b.LocalUser)
             .WithMany(u => u.Reviews)
             .HasForeignKey(b => b.LocalUserId);
    }
}