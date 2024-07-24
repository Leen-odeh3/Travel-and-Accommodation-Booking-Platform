
namespace HotelBookingPlatform.Domain.DTOs.LocalUser;
public class LocalUserDto
{
    public string UserName { get; set;}
    public string Email { get; set;}

    public static implicit operator LocalUserDto(Entities.LocalUser v)
    {
        throw new NotImplementedException();
    }
}
