namespace HotelBookingPlatform.Application.Core.Implementations;
public class BookingService : BaseService<Booking>, IBookingService
{
    public BookingService(IUnitOfWork<Booking> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }
    public async Task<BookingDto> GetBookingAsync(int id)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            throw new NotFoundException("Booking not found.");
        }

        Console.WriteLine($"Hotel: {booking.Hotel?.Name}");
        Console.WriteLine($"Rooms: {string.Join(", ", booking.Rooms.Select(r => r.Number))}");

        var bookingDto = _mapper.Map<BookingDto>(booking);
        return bookingDto;
    }


    public async Task<BookingDto> CreateBookingAsync(BookingCreateRequest request, string email)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
        if (user is null)
            throw new NotFoundException("User not found.");

        if (request.CheckInDateUtc >= request.CheckOutDateUtc)
            throw new BadRequestException("Check-out date must be greater than check-in date.");

        var totalPrice = await CalculateTotalPriceAsync(request.RoomIds.ToList(), request.CheckInDateUtc, request.CheckOutDateUtc);

        var booking = new Booking
        {
            UserId = user.Id,
            User = user,
            confirmationNumber = GenerateConfirmationNumber(),
            TotalPrice = totalPrice,
            BookingDateUtc = DateTime.UtcNow,
            PaymentMethod = request.PaymentMethod,
            HotelId = request.HotelId,
            CheckInDateUtc = request.CheckInDateUtc,
            CheckOutDateUtc = request.CheckOutDateUtc,
            Status = BookingStatus.Pending,
            Rooms = new List<Room>()
        };

        foreach (var roomId in request.RoomIds)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            if (room is not null)
            {
                booking.Rooms.Add(room);
            }
        }
        await _unitOfWork.BookingRepository.CreateAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BookingDto>(booking);
    }

    public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);
        if (booking is null)
        {
            throw new NotFoundException("Booking not found.");
        }

        booking.Status = newStatus;
        await _unitOfWork.BookingRepository.UpdateAsync(bookingId, booking);
        await _unitOfWork.SaveChangesAsync();
    }
    private async Task<decimal> CalculateTotalPriceAsync(List<int> roomIds, DateTime checkInDate, DateTime checkOutDate)
    {
        decimal totalPrice = 0m;
        foreach (var roomId in roomIds)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new BadRequestException($"Room with ID {roomId} not found.");
            }

            int numberOfNights = (checkOutDate - checkInDate).Days;
            totalPrice += room.PricePerNight * numberOfNights;
        }

        return totalPrice;
    }

    private string GenerateConfirmationNumber()
    {
        return Guid.NewGuid().ToString();
    }
}
