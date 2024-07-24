using HotelBookingPlatform.Domain.DTOs.LocalUser;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.Domain.DTOs.Login;
public class LoginResponseDto
{
    public string Token { get; set; }
    public LocalUserDto User { get; set; }

}