﻿using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;

namespace HotelBookingPlatform.Domain.Abstracts;
public interface IHotelRepository :IGenericRepository<Hotel>
{
    Task<IEnumerable<Hotel>> SearchCriteria(string name, string desc);

}
