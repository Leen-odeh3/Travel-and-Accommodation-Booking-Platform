namespace HotelBookingPlatform.Domain.Entities;
public class Review 
{
  public int ReviewID {  get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
}
