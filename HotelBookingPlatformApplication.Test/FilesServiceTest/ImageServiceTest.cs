namespace HotelBookingPlatformApplication.Test.FilesServiceTest;
public class ImageServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<Cloudinary> _mockCloudinary;
    private readonly Mock<IUnitOfWork<Image>> _mockUnitOfWork;
    private readonly ImageService _imageService;

    public ImageServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _mockCloudinary = new Mock<Cloudinary>(new Account("cloud", "apiKey", "apiSecret"));
        _mockUnitOfWork = _fixture.Freeze<Mock<IUnitOfWork<Image>>>();
        _imageService = new ImageService(_mockCloudinary.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task UploadImageAsync_WhenNoFileProvided_ShouldThrowArgumentException()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(0);

        Func<Task> act = async () => await _imageService.UploadImageAsync(file.Object, "folder", "entityType", 1);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("No file provided.");
    }

    [Fact]
    public async Task UploadImageAsync_WhenUnsupportedFileFormat_ShouldThrowArgumentException()
    {
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(1);
        file.Setup(f => f.FileName).Returns("file.txt");

        Func<Task> act = async () => await _imageService.UploadImageAsync(file.Object, "folder", "entityType", 1);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Unsupported file format.");
    }

    [Fact]
    public async Task GetImagesByTypeAsync_WhenCalled_ReturnsImages()
    {
        var type = "hotel";
        var expectedImages = _fixture.CreateMany<Image>(4);

        var mockImageRepository = new Mock<IImageRepository>();
        mockImageRepository
            .Setup(repo => repo.GetImagesByTypeAsync(type))
            .ReturnsAsync(expectedImages);

        _mockUnitOfWork.Setup(uow => uow.ImageRepository).Returns(mockImageRepository.Object);

        // Act
        var result = await _imageService.GetImagesByTypeAsync(type);
        result.Should().BeEquivalentTo(expectedImages);
    }

    [Fact]
    public async Task GetImageDetailsAsync_WhenPublicIdIsNullOrEmpty_ShouldThrowBadRequestException()
    {
        string invalidPublicId = string.Empty;
        Func<Task> act = async () => await _imageService.GetImageDetailsAsync(invalidPublicId);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Public ID cannot be null or empty.");
    }

    [Fact]
    public async Task DeleteImageAsync_WhenPublicIdIsNullOrEmpty_ShouldThrowBadRequestException()
    {
        var invalidPublicId = string.Empty;
        Func<Task> act = async () => await _imageService.DeleteImageAsync(invalidPublicId);

        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Public ID cannot be null or empty.");
    }

}
