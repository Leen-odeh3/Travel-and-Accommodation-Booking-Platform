namespace HotelBookingPlatform.Application.Core.Abstracts.HotelManagementService;
public interface IHotelManagementService
{
    Task<HotelResponseDto> GetHotel(int id);
    Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request);
    Task DeleteHotel(int id);
    Task<HotelResponseDto> CreateHotel(HotelCreateRequest request);
}


