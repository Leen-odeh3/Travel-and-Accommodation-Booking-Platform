namespace HotelBookingPlatform.Infrastructure.Implementation;
public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{    public AmenityRepository(AppDbContext context) : base(context)
    {
    }
}

