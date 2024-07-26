﻿using HotelBookingPlatform.Domain.Entities;
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

        builder.Property(r => r.ModifiedAtUtc)
               .IsRequired(false);

        builder.HasOne(r => r.RoomClass)
               .WithMany(rc => rc.Rooms)
               .HasForeignKey(r => r.RoomClassID);
                

    }
}