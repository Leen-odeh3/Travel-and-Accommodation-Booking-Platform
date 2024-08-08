using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface IUserRepository
{
    Task<LocalUser> GetUserByEmailAsync(string email);
    Task<LocalUser> GetByIdAsync(string userId);
}
