﻿namespace HotelBookingPlatform.API.Tests;
public class DiscountControllerTests
{
    private readonly Mock<IDiscountService> _discountServiceMock;
    private readonly Mock<ILogger<DiscountController>> _loggerMock;
    private readonly Mock<IResponseHandler> _responseHandlerMock;
    private readonly DiscountController _controller;

    public DiscountControllerTests()
    {
        _discountServiceMock = new Mock<IDiscountService>();
        _loggerMock = new Mock<ILogger<DiscountController>>();
        _responseHandlerMock = new Mock<IResponseHandler>();
        _controller = new DiscountController(
            _discountServiceMock.Object,
            _loggerMock.Object,
            _responseHandlerMock.Object
        );
    }

    [Fact]
    public async Task AddDiscount_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        var request = new DiscountCreateRequest
        {
            RoomID = 1,
            Percentage = 10,
            StartDateUtc = DateTime.UtcNow,
            EndDateUtc = DateTime.UtcNow.AddDays(7)
        };

        var discountDto = new DiscountDto { };
        _discountServiceMock
            .Setup(ds => ds.AddDiscountToRoomAsync(
                request.RoomID,
                request.Percentage,
                request.StartDateUtc,
                request.EndDateUtc))
            .ReturnsAsync(discountDto);

        _responseHandlerMock
            .Setup(rh => rh.Success(discountDto, "Discount added successfully."))
            .Returns(new OkObjectResult(discountDto));

        // Act
        var result = await _controller.AddDiscount(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<DiscountDto>(okResult.Value);
        Assert.Equal(discountDto, returnedDto);
    }

    [Fact]
    public async Task GetActiveDiscounts_ShouldReturnNotFound_WhenNoActiveDiscounts()
    {
        // Arrange
        var activeDiscounts = new List<DiscountDto>();

        _discountServiceMock
            .Setup(ds => ds.GetActiveDiscountsAsync())
            .ReturnsAsync(activeDiscounts);

        _responseHandlerMock
            .Setup(rh => rh.NotFound("No active discounts found."))
            .Returns(new NotFoundResult());

        // Act
        var result = await _controller.GetActiveDiscounts();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task GetRoomsWithHighestDiscounts_ShouldReturnDiscounts_WhenAvailable()
    {
        // Arrange
        var topN = 5;
        var roomsWithDiscounts = new List<DiscountDto>
    {
        new DiscountDto { DiscountID = 1, RoomNumber = "101", Percentage = 15, IsActive = true },
        new DiscountDto { DiscountID = 2, RoomNumber = "102", Percentage = 10, IsActive = false }
    };

        _discountServiceMock
            .Setup(ds => ds.GetTopDiscountsAsync(topN))
            .ReturnsAsync(roomsWithDiscounts);

        var successResponse = new OkObjectResult(new
        {
            statusCode = 200,
            succeeded = true,
            message = "Rooms with highest active discounts retrieved successfully.",
            data = roomsWithDiscounts
        });

        _responseHandlerMock
            .Setup(rh => rh.Success(roomsWithDiscounts, "Rooms with highest active discounts retrieved successfully."))
            .Returns(successResponse);

        // Act
        var result = await _controller.GetRoomsWithHighestDiscountsAsync(topN);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<dynamic>(okResult.Value);

        var returnedDiscounts = returnedValue.data;
        Assert.IsType<List<DiscountDto>>(returnedDiscounts);

        Assert.Equal(roomsWithDiscounts.Count, ((List<DiscountDto>)returnedDiscounts).Count);
        Assert.Equal("Rooms with highest active discounts retrieved successfully.", returnedValue.message);
    }



    [Fact]
    public async Task GetDiscountById_ShouldReturnNotFound_WhenDiscountDoesNotExist()
    {
        // Arrange
        var id = 999;
        _discountServiceMock
            .Setup(ds => ds.GetDiscountByIdAsync(id))
            .ReturnsAsync((DiscountDto)null);

        _responseHandlerMock
            .Setup(rh => rh.NotFound("Discount not found."))
            .Returns(new NotFoundResult());

        // Act
        var result = await _controller.GetDiscountById(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task DeleteDiscount_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var id = 1;
        _discountServiceMock
            .Setup(ds => ds.DeleteDiscountAsync(id))
            .ThrowsAsync(new Exception("Deletion failed"));

        _responseHandlerMock
            .Setup(rh => rh.BadRequest("Failed to delete discount."))
            .Returns(new BadRequestObjectResult("Failed to delete discount."));

        // Act
        var result = await _controller.DeleteDiscount(id);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Failed to delete discount.", badRequestResult.Value);
    }
}
