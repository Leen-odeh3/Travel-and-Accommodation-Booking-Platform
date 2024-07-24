using HotelBookingPlatform.Domain.DTOs.LocalUser;
using HotelBookingPlatform.Domain.DTOs.Login;
using HotelBookingPlatform.Domain.DTOs.Register;
namespace HotelBookingPlatform.Domain.IRepositories;
public interface IUserRepository
{
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<LocalUserDto> Register(RegisterRequestDto registerRequestDto);
    Task<bool> IsUniqueUser(string email);
}
