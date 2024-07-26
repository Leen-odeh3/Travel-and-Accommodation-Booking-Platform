﻿using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class OwnerRepository :GenericRepository<Owner> ,IOwnerRepository
{
    public OwnerRepository(AppDbContext context) : base(context)
    {
    }
}