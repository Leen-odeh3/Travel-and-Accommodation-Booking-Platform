using HotelBookingPlatform.Application.Core.Implementations.HotelManagementService;
namespace HotelBookingPlatformApplication.Test.ServicesTest.HotelManagementServiceTest;
public class HotelManagementServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IUnitOfWork<Hotel>> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelManagementService _service;
    public HotelManagementServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork<Hotel>>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _service = new HotelManagementService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetHotel_ShouldReturnHotel_WhenHotelExists()
    {
        // Arrange
        var hotel = _fixture.Create<Hotel>();
        var hotelDto = _fixture.Create<HotelResponseDto>();

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetByIdAsync(hotel.HotelId))
                       .ReturnsAsync(hotel);
        _mapperMock.Setup(m => m.Map<HotelResponseDto>(hotel))
                   .Returns(hotelDto);

        var result = await _service.GetHotel(hotel.HotelId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hotelDto, result);
    }

    [Fact]
    public async Task CreateHotel_ShouldReturnCreatedHotel_WhenRequestIsValid()
    {
        // Arrange
        var request = _fixture.Create<HotelCreateRequest>();
        var hotel = _fixture.Create<Hotel>();
        var hotelDto = _fixture.Create<HotelResponseDto>();

        _mapperMock.Setup(m => m.Map<Hotel>(request))
                   .Returns(hotel);
        _unitOfWorkMock.Setup(u => u.HotelRepository.CreateAsync(hotel))
                       .Returns(Task.FromResult(hotel));  
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                       .Returns(Task.FromResult(1));
        _mapperMock.Setup(m => m.Map<HotelResponseDto>(hotel))
                   .Returns(hotelDto);

        var result = await _service.CreateHotel(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(hotelDto, result);
    }

    [Fact]
    public async Task UpdateHotelAsync_ShouldUpdateHotel_WhenHotelExists()
    {
        // Arrange
        var id = 1;
        var request = _fixture.Create<HotelResponseDto>();
        var existingHotel = _fixture.Create<Hotel>();
        var updatedHotel = _fixture.Create<HotelResponseDto>();

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetByIdAsync(id))
                       .ReturnsAsync(existingHotel);
        _mapperMock.Setup(m => m.Map(request, existingHotel));  
        _unitOfWorkMock.Setup(u => u.HotelRepository.UpdateAsync(id, existingHotel))
                       .Returns(Task.FromResult(existingHotel)); 
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                       .Returns(Task.FromResult(1));
        _mapperMock.Setup(m => m.Map<HotelResponseDto>(existingHotel))
                   .Returns(updatedHotel);

        // Act
        var result = await _service.UpdateHotelAsync(id, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedHotel, result);
    }


    [Fact]
    public async Task DeleteHotel_ShouldDeleteHotel_WhenHotelExists()
    {
        var id = 1;
        var hotel = _fixture.Create<Hotel>();

        _unitOfWorkMock.Setup(u => u.HotelRepository.GetByIdAsync(id))
                       .ReturnsAsync(hotel);
        _unitOfWorkMock.Setup(u => u.HotelRepository.DeleteAsync(id))
                       .Returns(Task.FromResult("Success")); 
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                       .Returns(Task.FromResult(1));

        await _service.DeleteHotel(id);

        _unitOfWorkMock.Verify(u => u.HotelRepository.DeleteAsync(id), Times.Once);
    }

}
