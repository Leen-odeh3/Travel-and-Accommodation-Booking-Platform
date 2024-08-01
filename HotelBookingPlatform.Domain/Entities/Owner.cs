namespace HotelBookingPlatform.Domain.Entities;
public class Owner 
{
    public int OwnerID {  get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int HotelCount => Hotels.Count;
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}
