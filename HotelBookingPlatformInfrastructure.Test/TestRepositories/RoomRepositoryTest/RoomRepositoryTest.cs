namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.RoomRepositoryTest;
public class RoomRepositoryTest
{
    private readonly RoomRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly IFixture _fixture;

    public RoomRepositoryTest()
    {
        _context = new InMemoryDbContext();
        _sut = new RoomRepository(_context);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task AddRoom_ShouldAddRoomToDatabase()
    {
        // Arrange
        var room = _fixture.Create<Room>();

        // Act
        await _sut.CreateAsync(room);
        var addedRoom = await _context.Rooms.FindAsync(room.RoomID);

        // Assert
        Assert.NotNull(addedRoom);
        Assert.Equal(room.RoomID, addedRoom.RoomID);
    }

    [Fact]
    public async Task GetRoomById_ShouldReturnRoom()
    {
        // Arrange
        var room = _fixture.Create<Room>();
        await _sut.CreateAsync(room);

        // Act
        var retrievedRoom = await _sut.GetByIdAsync(room.RoomID);

        // Assert
        Assert.NotNull(retrievedRoom);
        Assert.Equal(room.RoomID, retrievedRoom.RoomID);
    }

    [Fact]
    public async Task UpdateRoom_ShouldUpdateRoomInDatabase()
    {
        // Arrange
        var room = _fixture.Create<Room>();
        await _sut.CreateAsync(room);
        room.Number = "G012";

        await _sut.UpdateAsync(room.RoomID ,room);
        var updatedRoom = await _sut.GetByIdAsync(room.RoomID);

        // Assert
        Assert.NotNull(updatedRoom);
        Assert.Equal("G012", updatedRoom.Number);
    }
}

