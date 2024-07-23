using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class BookingRepository :GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context):base(context)
    {
        
    }

}
