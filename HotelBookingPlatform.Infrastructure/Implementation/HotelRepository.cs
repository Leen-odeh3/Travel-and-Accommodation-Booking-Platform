﻿using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class HotelRepository : GenericRepository<Hotel> , IHotelRepository
{
    public HotelRepository(AppDbContext context) : base(context)
    {
       
    }
    public async Task<IEnumerable<Hotel>> SearchCriteria(
      string name,
      string desc,
      int pageSize = 10,
      int pageNumber = 1)
    {
        return await _appDbContext.Set<Hotel>()
            .Where(h => (string.IsNullOrEmpty(name) || h.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(desc) || h.Description.Contains(desc)))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

}
