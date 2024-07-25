using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Implementation;

namespace HotelBookingPlatform.Infrastructure;
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        HotelRepository = new HotelRepository(_context);
        BookingRepository = new BookingRepository(_context);
        RoomClasseRepository = new RoomClassRepository(_context);
        RoomRepository = new RoomRepository(_context);
        CityRepository= new CityRepository(_context);
        OwnerRepository = new OwnerRepository(_context);
        DiscountRepository = new DiscountRepository(_context);

    }

    public IHotelRepository HotelRepository { get; }

    public IBookingRepository BookingRepository { get; }

    public IRoomClasseRepository RoomClasseRepository { get; }

    public IRoomRepository RoomRepository { get; }

    public ICityRepository CityRepository { get; }

    public IOwnerRepository OwnerRepository { get; }
    public IDiscountRepository DiscountRepository { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

