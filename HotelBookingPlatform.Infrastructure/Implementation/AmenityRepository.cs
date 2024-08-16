namespace HotelBookingPlatform.Infrastructure.Implementation;
public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{
    private readonly ILog _logger;
    public AmenityRepository(AppDbContext context, ILog logger)
      : base(context, logger)
    {
        _logger = logger;
    }

}

