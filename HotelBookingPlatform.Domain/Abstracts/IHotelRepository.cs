namespace HotelBookingPlatform.Domain.Abstracts;
public interface IHotelRepository :IGenericRepository<Hotel>
{
    Task<IEnumerable<Hotel>> SearchCriteria(string name, string desc, int pageSize = 10, int pageNumber = 1);
    Task<Hotel> GetHotelByNameAsync(string name);
    Task<IEnumerable<Hotel>> GetHotelsForCityAsync(int cityId);
    Task<Hotel> GetHotelWithRoomClassesAndRoomsAsync(int hotelId);
    Task<Hotel> GetHotelWithAmenitiesAsync(int hotelId);
    Task<IEnumerable<Hotel>> SearchHotelsAsync(
       string? cityName,
       int numberOfAdults,
       int numberOfChildren,
       int numberOfRooms,
       DateTime checkInDate,
       DateTime checkOutDate,
       int? starRating
   );
}
