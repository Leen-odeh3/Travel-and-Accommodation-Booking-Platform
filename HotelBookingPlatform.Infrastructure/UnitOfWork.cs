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
    }

    public IHotelRepository HotelRepository { get; }

    public IBookingRepository BookingRepository { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

