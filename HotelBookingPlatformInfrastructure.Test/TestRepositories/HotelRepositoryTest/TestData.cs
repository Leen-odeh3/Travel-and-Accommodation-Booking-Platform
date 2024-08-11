namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.HotelRepositoryTest;
public static class TestData
{
    public static Hotel CreateHotel(int hotelId, int cityId, int ownerId)
    {
        return new Hotel
        {
            HotelId = hotelId,
            Name = hotelId % 2 == 0 ? "Hilton Garden Inn" : "The Ritz-Carlton",
            StarRating = hotelId % 2 == 0 ? 4 : 5,
            Description = hotelId % 2 == 0 ? "Modern hotel with comfortable rooms and excellent amenities." : "Luxurious hotel offering top-notch services and elegant accommodations.",
            PhoneNumber = "+1-800-555-" + hotelId.ToString("D4"),
            CreatedAtUtc = DateTime.UtcNow,
            CityID = cityId,
            OwnerID = ownerId
        };
    }

    public static Amenity CreateAmenity(int amenityId, int hotelId)
    {
        return new Amenity
        {
            AmenityID = amenityId,
            Name = amenityId % 3 == 0 ? "Outdoor Pool" : (amenityId % 3 == 1 ? "Free Breakfast" : "Spa Services"),
            HotelId = hotelId,
            Description = amenityId % 3 == 0 ? "Relax by the pool with stunning views." : (amenityId % 3 == 1 ? "Enjoy a complimentary breakfast buffet each morning." : "Indulge in a range of spa treatments and massages.")
        };
    }

    public static RoomClass CreateRoomClass(int roomClassId, int hotelId)
    {
        return new RoomClass
        {
            RoomClassID = roomClassId,
            Name = roomClassId % 2 == 0 ? "Executive Suite" : "Standard Room",
            HotelId = hotelId
        };
    }

    public static Room CreateRoom(int roomId, int roomClassId)
    {
        return new Room
        {
            RoomID = roomId,
            Number = "Room " + roomId.ToString("D3"),
            RoomClassID = roomClassId
        };
    }
}

