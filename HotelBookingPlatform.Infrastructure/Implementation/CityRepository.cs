using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class CityRepository :GenericRepository<City> , ICityRepository
{
    public CityRepository(AppDbContext context) : base(context)
    {

    }
}
