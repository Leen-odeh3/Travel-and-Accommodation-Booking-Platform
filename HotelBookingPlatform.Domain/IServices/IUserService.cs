using HotelBookingPlatform.Domain.Helpers;
namespace HotelBookingPlatform.Domain.IServices;
public interface IUserService
{
    Task<AuthModel> RegisterAsync(RegisterModel model);
    Task<AuthModel> LoginAsync(LoginModel model);
}
