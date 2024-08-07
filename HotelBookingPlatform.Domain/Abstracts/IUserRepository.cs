using HotelBookingPlatform.Domain.Entities;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface IUserRepository
{
    Task<LocalUser> GetByEmailAsync(string email);

}
