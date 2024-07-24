using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;

namespace HotelBookingPlatform.Domain.Abstracts;
public interface IBookingRepository:IGenericRepository<Booking>
{
}
