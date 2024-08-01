using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{    public AmenityRepository(AppDbContext context) : base(context)
    {
    }
}

