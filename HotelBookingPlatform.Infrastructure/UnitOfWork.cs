using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Implementation;
namespace HotelBookingPlatform.Infrastructure;
public class UnitOfWork<T> : IUnitOfWork<T> where T :class
{
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        HotelRepository = new HotelRepository(_context);
        BookingRepository = new BookingRepository(_context);
        RoomClasseRepository = new RoomClassRepository(_context);
        RoomRepository = new RoomRepository(_context);
        CityRepository = new CityRepository(_context);
        OwnerRepository = new OwnerRepository(_context);
        DiscountRepository = new DiscountRepository(_context);
        ReviewRepository = new ReviewRepository(_context);
        InvoiceRecordRepository =new InvoiceRecordRepository(_context);
        AmenityRepository = new AmenityRepository(_context);
        ImageRepository= new ImageRepository(_context);
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
    public IImageRepository ImageRepository { get; set; }
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();   
}

