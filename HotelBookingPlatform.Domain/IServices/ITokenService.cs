using HotelBookingPlatform.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
namespace HotelBookingPlatform.Domain.IServices;
public interface ITokenService
{
    Task<JwtSecurityToken> CreateJwtToken(LocalUser user);
}
