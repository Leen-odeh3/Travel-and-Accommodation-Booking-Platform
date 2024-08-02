namespace HotelBookingPlatform.Domain.DTOs.HomePage;
public class SearchRequestDto
{
    public DateTime CheckInDate { get; set; } = DateTime.Today;
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
    public int NumberOfAdults { get; set; } = 2;
    public int NumberOfChildren { get; set; } = 0;
    public int NumberOfRooms { get; set; } = 1;
    public string? CityName { get; set; }
    public int? StarRating { get; set; }
}