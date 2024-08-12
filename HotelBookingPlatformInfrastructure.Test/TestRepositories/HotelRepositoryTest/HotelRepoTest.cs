namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.HotelRepositoryTest;
public class HotelRepoTest
{
    private readonly HotelRepository _sut;
    private readonly InMemoryDbContext _context;

    public HotelRepoTest()
    {
        _context = new InMemoryDbContext();
        _sut = new HotelRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_WhenHotelsExist_ShouldReturnHotels()
    {
        var hotel = TestData.CreateHotel(1, 101, 202);
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Any(h => h.HotelId == hotel.HotelId), "Hotel should be present in the result.");
    }

    [Fact]
    public async Task GetHotelWithAmenitiesAsync_WhenHotelExists_ShouldReturnHotelWithAmenities()
    {
        var hotel = TestData.CreateHotel(1, 101, 202);
        var amenity = TestData.CreateAmenity(1, 1);

        hotel.Amenities = new List<Amenity> { amenity };
        _context.Hotels.Add(hotel);
        _context.Amenities.Add(amenity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelWithAmenitiesAsync(hotel.HotelId);

        // Assert
        Assert.Equal(hotel.HotelId, result.HotelId);
        Assert.NotNull(result.Amenities);
        Assert.Contains(result.Amenities, a => a.AmenityID == amenity.AmenityID && a.Name == amenity.Name);
    }

    [Fact]
    public async Task GetHotelByNameAsync_WhenHotelExists_ShouldReturnHotelWithRoomClassesAndAmenities()
    {
        var hotel = TestData.CreateHotel(1, 101, 202);
        var roomClass = TestData.CreateRoomClass(1, 1);
        var amenity = TestData.CreateAmenity(1, 1);

        roomClass.Amenities = new List<Amenity> { amenity };
        hotel.RoomClasses = new List<RoomClass> { roomClass };

        _context.Hotels.Add(hotel);
        _context.RoomClasses.Add(roomClass);
        _context.Amenities.Add(amenity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelByNameAsync(hotel.Name);

        // Assert
        Assert.Equal(hotel.Name, result.Name);
        Assert.NotNull(result.RoomClasses);
        Assert.Contains(result.RoomClasses, rc => rc.RoomClassID == roomClass.RoomClassID);
        Assert.NotNull(result.RoomClasses.First().Amenities);
        Assert.Contains(result.RoomClasses.First().Amenities, a => a.AmenityID == amenity.AmenityID);
    }

    [Fact]
    public async Task GetAllAsync_WithInvalidPageSizeOrPageNumber_ShouldReturnEmptyList()
    {
        var hotel1 = TestData.CreateHotel(1, 101, 202);
        var hotel2 = TestData.CreateHotel(2, 102, 203);

        _context.Hotels.AddRange(hotel1, hotel2);
        await _context.SaveChangesAsync();

        // Act
        var resultNegativePageSize = await _sut.GetAllAsync(-1, 1);
        var resultZeroPageSize = await _sut.GetAllAsync(0, 1);
        var resultNegativePageNumber = await _sut.GetAllAsync(10, -1);
        var resultZeroPageNumber = await _sut.GetAllAsync(10, 0);

        // Assert
        Assert.NotNull(resultNegativePageSize);
        Assert.Empty(resultNegativePageSize);

        Assert.NotNull(resultZeroPageSize);
        Assert.Empty(resultZeroPageSize);

        Assert.NotNull(resultNegativePageNumber);
        Assert.Empty(resultNegativePageNumber);

        Assert.NotNull(resultZeroPageNumber);
        Assert.Empty(resultZeroPageNumber);
    }

    [Fact]
    public async Task GetAllAsync_WithValidPageSizeAndPageNumber_ShouldReturnHotels()
    {
        var hotel1 = TestData.CreateHotel(1, 101, 202);
        var hotel2 = TestData.CreateHotel(2, 102, 203);

        _context.Hotels.AddRange(hotel1, hotel2);
        await _context.SaveChangesAsync(); 

        var hotelsInDb = await _context.Hotels.ToListAsync();
        Assert.Equal(2, hotelsInDb.Count);    
    }

    [Fact]
    public async Task GetHotelWithRoomClassesAndRoomsAsync_WithValidHotelId_ShouldReturnHotelWithRoomClassesAndRooms()
    {
        var hotel = TestData.CreateHotel(1, 101, 202);
        var roomClass1 = TestData.CreateRoomClass(1, 1);
        var roomClass2 = TestData.CreateRoomClass(2, 1);

        var room1 = TestData.CreateRoom(1, 1);
        var room2 = TestData.CreateRoom(2, 1);
        var room3 = TestData.CreateRoom(3, 2);

        roomClass1.Rooms = new List<Room> { room1, room2 };
        roomClass2.Rooms = new List<Room> { room3 };

        hotel.RoomClasses = new List<RoomClass> { roomClass1, roomClass2 };

        _context.Hotels.Add(hotel);
        _context.RoomClasses.AddRange(roomClass1, roomClass2);
        _context.Rooms.AddRange(room1, room2, room3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelWithRoomClassesAndRoomsAsync(hotel.HotelId);

        // Assert
        Assert.Equal(hotel.HotelId, result.HotelId);
        Assert.NotEmpty(result.RoomClasses);
        Assert.Equal(2, result.RoomClasses.Count);
        Assert.Contains(result.RoomClasses, rc => rc.Rooms.Count == 2);
    }

    [Fact]
    public async Task GetHotelWithRoomClassesAndRoomsAsync_WithInvalidHotelId_ShouldReturnNull()
    {
        var invalidHotelId = 999;

        // Act
        var result = await _sut.GetHotelWithRoomClassesAndRoomsAsync(invalidHotelId);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetHotelsForCityAsync_WithValidCityId_ShouldReturnHotels()
    {
        var cityId = 101;
        var hotel1 = TestData.CreateHotel(1, cityId, 202);
        var hotel2 = TestData.CreateHotel(2, cityId, 203);

        _context.Hotels.AddRange(hotel1, hotel2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetHotelsForCityAsync(cityId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, h => h.HotelId == hotel1.HotelId);
    }

    [Fact]
    public async Task GetHotelsForCityAsync_WithInvalidCityId_ShouldReturnEmptyList()
    {
        var invalidCityId = 999;

        // Act
        var result = await _sut.GetHotelsForCityAsync(invalidCityId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

