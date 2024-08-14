namespace HotelBookingPlatform.Infrastructure.Implementation;
public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{
    private readonly ILogger _logger;
    public AmenityRepository(AppDbContext context, ILogger logger)
      : base(context, logger)
    {
        _logger = logger;
    }

}

