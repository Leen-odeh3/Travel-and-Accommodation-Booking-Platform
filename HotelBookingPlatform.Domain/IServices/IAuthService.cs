using HotelBookingPlatform.Domain.Helpers;
namespace HotelBookingPlatform.Domain.IServices;
public interface IAuthService
{
    Task<AuthModel> RegisterAsync(RegisterModel model);
    Task<AuthModel> GetTokenAsync(LoginModel model);
    Task<string> AddRoleAsync(AddRoleModel model);
}
