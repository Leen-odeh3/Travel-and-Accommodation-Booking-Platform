namespace HotelBookingPlatform.Infrastructure.Implementation;
public class DiscountRepository : GenericRepository<Discount> ,IDiscountRepository
{

    public DiscountRepository(AppDbContext context) : base(context)
    {
    }

}
