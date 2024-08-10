
namespace HotelBookingPlatform.Domain;
public interface IUnitOfWork<T>  where T: class 
{
    IHotelRepository HotelRepository { get; set; }
    IBookingRepository BookingRepository { get; set; }
    IRoomClasseRepository RoomClasseRepository { get; set; }
    IRoomRepository RoomRepository { get; set; }
    ICityRepository CityRepository { get; set; }
    IOwnerRepository OwnerRepository { get; set; }
    IDiscountRepository DiscountRepository { get; set; }
    IReviewRepository ReviewRepository { get; set; }
    IInvoiceRecordRepository InvoiceRecordRepository { get; set; }
    IAmenityRepository AmenityRepository { get; set; }
    IImageRepository ImageRepository { get; set; }
    IUserRepository UserRepository { get; set; }
    Task<int> SaveChangesAsync();
}