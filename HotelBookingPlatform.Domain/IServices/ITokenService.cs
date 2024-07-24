using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.Domain.IServices;
public interface ITokenService
{
    public Task<String> CreateTokenAsync(LocalUser localUser);
}
