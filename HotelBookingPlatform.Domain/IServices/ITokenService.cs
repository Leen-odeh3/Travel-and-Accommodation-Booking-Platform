using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;
namespace HotelBookingPlatform.Domain.IServices;
public interface ITokenService
{
    Task<JwtSecurityToken> CreateJwtToken(LocalUser user);
    Task<AuthModel> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
    RefreshToken GenerateRefreshToken();
    void SetRefreshTokenInCookie(string refreshToken, DateTime expires);
}
