namespace HotelBookingPlatform.Application.Core.Implementations
{
    public class DiscountService : BaseService<Discount>, IDiscountService
    {
        public DiscountService(IUnitOfWork<Discount> unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        public async Task<DiscountDto> AddDiscountToRoomAsync(int roomId, decimal percentage, DateTime startDateUtc, DateTime endDateUtc)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);

            if (room is null)
            {
                throw new Exception("Room not found.");
            }

            var discount = new Discount
            {
                RoomID = roomId,
                Percentage = percentage,
                StartDateUtc = startDateUtc,
                EndDateUtc = endDateUtc
            };

            await _unitOfWork.DiscountRepository.CreateAsync(discount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DiscountDto>(discount);
        }


    }
}
