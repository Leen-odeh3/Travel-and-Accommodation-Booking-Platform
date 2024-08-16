﻿namespace HotelBookingPlatform.Domain.Entities;
public class Image
{
    public int Id { get; set; }
    public string PublicId { get; set; }
    public string Url { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
