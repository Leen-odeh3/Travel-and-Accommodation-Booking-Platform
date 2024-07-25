using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomClassRepository :GenericRepository<RoomClass> , IRoomClasseRepository
{
    public RoomClassRepository(AppDbContext context) : base(context)
    {

    }
}
