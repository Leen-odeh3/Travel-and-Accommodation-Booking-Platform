using Microsoft.AspNetCore.Identity;

namespace HotelBookingPlatform.Domain.Entities;
public class LocalUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<Review> Reviews { get; set; }

}
