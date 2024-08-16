using ILog = HotelBookingPlatform.Domain.ILogger.ILog;
namespace HotelBookingPlatform.Infrastructure;
public class UnitOfWork<T> : IUnitOfWork<T> where T :class
{
    private readonly AppDbContext _context;
    private readonly UserManager<LocalUser> _userManager;
    private readonly ILog _logger;

    public UnitOfWork(AppDbContext context, UserManager<LocalUser> userManager)
    {
        _context = context;
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        HotelRepository = new HotelRepository(_context,_logger);
        BookingRepository = new BookingRepository(_context, _logger);
        RoomClasseRepository = new RoomClassRepository(_context, _logger);
        RoomRepository = new RoomRepository(_context, _logger);
        CityRepository = new CityRepository(_context, _logger);
        OwnerRepository = new OwnerRepository(_context,_logger);
        DiscountRepository = new DiscountRepository(_context, _logger);
        ReviewRepository = new ReviewRepository(_context, _logger);
        InvoiceRecordRepository =new InvoiceRecordRepository(_context, _logger);
        AmenityRepository = new AmenityRepository(_context, _logger);
        UserRepository = new UserRepository(_userManager,_context, _logger);
        ImageRepository = new ImageRepository(_context, _logger);
    }
    public IHotelRepository HotelRepository { get; set;}
    public IBookingRepository BookingRepository { get; set;}
    public IRoomClasseRepository RoomClasseRepository { get; set;}
    public IRoomRepository RoomRepository { get; set;}
    public ICityRepository CityRepository { get; set;}
    public IOwnerRepository OwnerRepository { get; set; }
    public IDiscountRepository DiscountRepository { get; set; }
    public IReviewRepository ReviewRepository { get; set; }
    public IInvoiceRecordRepository InvoiceRecordRepository {get; set; }
    public IAmenityRepository AmenityRepository { get; set;}
    public IUserRepository UserRepository { get; set; }
    public IImageRepository ImageRepository { get; set; }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();   
}

