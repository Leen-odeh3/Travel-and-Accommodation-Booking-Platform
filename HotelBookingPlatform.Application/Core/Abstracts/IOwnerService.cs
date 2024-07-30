using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Owner;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IOwnerService
{
    Task<OwnerDto> GetOwnerAsync(int id);
    Task<OwnerDto> CreateOwnerAsync(OwnerCreateDto request);
    Task<OwnerDto> UpdateOwnerAsync(int id, OwnerDto request);
    Task<string> DeleteOwnerAsync(int id);
}
