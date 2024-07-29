using HotelBookingPlatform.Domain.DTOs.LocalUser;
namespace HotelBookingPlatform.Domain.DTOs.Login;
public class LoginResponseDto
{
    public string Token { get; set; }
    public LocalUserDto User { get; set; }
}