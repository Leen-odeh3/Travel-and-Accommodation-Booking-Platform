using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Owner;
namespace HotelBookingPlatform.Application.Core.Abstracts;
public interface IOwnerService
{
    Task<Response<OwnerDto>> GetOwnerAsync(int id);
    Task<Response<OwnerDto>> CreateOwnerAsync(OwnerCreateDto request);
    Task<Response<OwnerDto>> UpdateOwnerAsync(int id, OwnerDto request);
    Task<Response<string>> DeleteOwnerAsync(int id);
}
