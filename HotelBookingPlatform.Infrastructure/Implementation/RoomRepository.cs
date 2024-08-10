using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class RoomRepository :GenericRepository<Room> ,IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context)
    {
        
    }

}
