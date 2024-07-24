using HotelBookingPlatform.Domain.DTOs.LocalUser;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.Domain.DTOs.Login;
public class LoginResponseDto
{
    public string token { get; set; }
    public LocalUserDto User { get; set; }
    public string Role { get; set; }
}
