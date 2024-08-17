namespace HotelBookingPlatform.Infrastructure.Test.TestRepositories;
public class RoomClassRepositoryTest
{
    private readonly RoomClassRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly IFixture _fixture;

    public RoomClassRepositoryTest()
    {
        _context = new InMemoryDbContext();
        _sut = new RoomClassRepository(_context);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task SearchCriteria_ShouldReturnFilteredRoomClasses()
    {
        // Arrange
        var roomClass1 = _fixture.Build<RoomClass>()
            .With(rc => rc.Name, "Deluxe")
            .With(rc => rc.Description, "Spacious and luxurious")
            .Create();
        var roomClass2 = _fixture.Build<RoomClass>()
            .With(rc => rc.Name, "Standard")
            .With(rc => rc.Description, "Comfortable and cozy")
            .Create();

        _context.RoomClasses.Add(roomClass1);
        _context.RoomClasses.Add(roomClass2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.SearchCriteria("Deluxe", null);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, rc => rc.Name == "Deluxe");
    }

    [Fact]
    public async Task SearchCriteria_ShouldPaginateResults()
    {
        for (int i = 1; i <= 20; i++)
        {
            _context.RoomClasses.Add(_fixture.Build<RoomClass>()
                .With(rc => rc.Name, $"RoomClass{i}")
                .Create());
        }
        await _context.SaveChangesAsync();
        var result = await _sut.SearchCriteria(null, null, pageSize: 10, pageNumber: 2);

        // Assert
        Assert.Equal(10, result.Count());
        Assert.Contains(result, rc => rc.Name == "RoomClass11");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRoomClasses()
    {
        // Arrange
        var roomClass1 = _fixture.Build<RoomClass>().Create();
        var roomClass2 = _fixture.Build<RoomClass>().Create();

        _context.RoomClasses.Add(roomClass1);
        _context.RoomClasses.Add(roomClass2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, rc => rc.RoomClassID == roomClass1.RoomClassID);
        Assert.Contains(result, rc => rc.RoomClassID == roomClass2.RoomClassID);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRoomClassWithHotel()
    {
        // Arrange
        var hotel = _fixture.Build<Hotel>()
            .With(h => h.Name, "Chic Paris Inn") 
            .With(h => h.PhoneNumber, "+123-456-7890") 
            .Create();

        var roomClass = _fixture.Build<RoomClass>()
            .With(rc => rc.Hotel, hotel)
            .Create();

        _context.RoomClasses.Add(roomClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(roomClass.RoomClassID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomClass.RoomClassID, result.RoomClassID);
        Assert.NotNull(result.Hotel);
        Assert.Equal(hotel.Name, result.Hotel.Name);
        Assert.Equal(hotel.PhoneNumber, result.Hotel.PhoneNumber);
    }

    [Fact]
    public async Task GetRoomClassWithAmenitiesAsync_ShouldReturnRoomClassWithAmenities()
    {
        // Arrange
        var amenities = new List<Amenity>
    {
        _fixture.Build<Amenity>()
            .With(a => a.Name, "Free Wi-Fi") 
            .With(a => a.Description, "Complimentary Wi-Fi available throughout the hotel")
            .Create()
    };

        var roomClass = _fixture.Build<RoomClass>()
            .With(rc => rc.Amenities, amenities)
            .Create();

        _context.RoomClasses.Add(roomClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoomClassWithAmenitiesAsync(roomClass.RoomClassID);

        Assert.NotEmpty(result.Amenities);
        Assert.Contains(result.Amenities, a => a.Name == "Free Wi-Fi" && a.Description == "Complimentary Wi-Fi available throughout the hotel");
    }

    [Fact]
    public async Task GetRoomClassWithRoomsAsync_ShouldReturnRoomClassWithRooms()
    {
        // Arrange
        var room = _fixture.Build<Room>().Create(); 
          
        var roomClass = _fixture.Build<RoomClass>()
            .With(rc => rc.Rooms, new List<Room> { room })
            .Create();

        _context.RoomClasses.Add(roomClass);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoomClassWithRoomsAsync(roomClass.RoomClassID);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Rooms);
        Assert.Contains(result.Rooms, r => r.RoomID == room.RoomID); 
    }

}
