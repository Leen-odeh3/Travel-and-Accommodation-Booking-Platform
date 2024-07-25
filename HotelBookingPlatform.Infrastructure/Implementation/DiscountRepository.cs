using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
namespace HotelBookingPlatform.Infrastructure.Implementation;
public class DiscountRepository : GenericRepository<Discount> ,IDiscountRepository
{

    public DiscountRepository(AppDbContext context) : base(context)
    {
    }

}
