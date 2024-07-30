namespace HotelBookingPlatform.Domain.DTOs.Owner;
public class OwnerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int HotelCount { get; set; }
}
