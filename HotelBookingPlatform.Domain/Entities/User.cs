namespace HotelBookingPlatform.Domain.Entities;
public class User
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public String Role { get; set; }
    public ICollection<Booking> Bookings { get; set; }

}
