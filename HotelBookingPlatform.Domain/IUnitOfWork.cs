namespace HotelBookingPlatform.Domain;
public interface IUnitOfWork<T> where T : class
{
    IHotelRepository HotelRepository { get; }
    IBookingRepository BookingRepository { get; }
    IRoomClasseRepository RoomClasseRepository { get; }
    IRoomRepository RoomRepository { get; }
    ICityRepository CityRepository { get; }
    IOwnerRepository OwnerRepository { get; }
    IImageRepository ImageRepository { get; }
    IDiscountRepository DiscountRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IInvoiceRecordRepository InvoiceRecordRepository { get; }
    IAmenityRepository AmenityRepository { get; }
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();
}