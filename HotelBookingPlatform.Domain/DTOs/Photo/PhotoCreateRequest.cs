using Microsoft.AspNetCore.Http;
namespace HotelBookingPlatform.Domain.DTOs.Photo;
public class PhotoCreateRequest
{
    public IFormFile PhotoFile { get; set; }
}

