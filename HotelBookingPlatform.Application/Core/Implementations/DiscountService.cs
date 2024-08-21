namespace HotelBookingPlatform.Application.Core.Implementations;
public class DiscountService : BaseService<Discount>, IDiscountService
{
    public DiscountService(IUnitOfWork<Discount> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }

    public async Task<DiscountDto> AddDiscountToRoomAsync(int roomId, decimal percentage, DateTime startDateUtc, DateTime endDateUtc)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
        var discount = new Discount
        {
            RoomID = roomId,
            Percentage = percentage,
            StartDateUtc = startDateUtc,
            EndDateUtc = endDateUtc,
            IsActive = CalculateIsActive(startDateUtc, endDateUtc)
        };

        await _unitOfWork.DiscountRepository.CreateAsync(discount);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DiscountDto>(discount);
    }

    public async Task<List<DiscountDto>> GetActiveDiscountsAsync()
    {
        var now = DateTime.UtcNow;
        var discounts = await _unitOfWork.DiscountRepository.GetAllAsync();
        var activeDiscounts = discounts.Where(d => CalculateIsActive(d.StartDateUtc, d.EndDateUtc)).ToList();

        return _mapper.Map<List<DiscountDto>>(activeDiscounts);
    }

    public async Task<List<DiscountDto>> GetAllDiscountsAsync()
    {
        var discounts = await _unitOfWork.DiscountRepository.GetAllAsync(query => query.Include(d => d.Room));
        var now = DateTime.UtcNow;
        var discountDtos = _mapper.Map<List<DiscountDto>>(discounts);
        foreach (var dto in discountDtos)
        {
            dto.IsActive = CalculateIsActive(dto.StartDateUtc, dto.EndDateUtc);
        }

        return discountDtos;
    }

    public async Task<DiscountDto> GetDiscountByIdAsync(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id, query => query.Include(d => d.Room));
        var discountDto = _mapper.Map<DiscountDto>(discount);
        discountDto.IsActive = CalculateIsActive(discount.StartDateUtc, discount.EndDateUtc);

        return discountDto;
    }

    public async Task DeleteDiscountAsync(int id)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount == null) throw new NotFoundException($"Discount with ID {id} not found.");

        await _unitOfWork.DiscountRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<DiscountDto> UpdateDiscountAsync(int id, UpdateDiscountRequest request)
    {
        var discount = await _unitOfWork.DiscountRepository.GetByIdAsync(id);
        if (discount != null)
        {
            if (request.Percentage.HasValue)
            {
                discount.Percentage = request.Percentage.Value;
            }
            if (request.StartDateUtc.HasValue)
            {
                discount.StartDateUtc = request.StartDateUtc.Value;
            }
            if (request.EndDateUtc.HasValue)
            {
                discount.EndDateUtc = request.EndDateUtc.Value;
            }

            discount.IsActive = DateTime.UtcNow >= discount.StartDateUtc && DateTime.UtcNow <= discount.EndDateUtc;

            _unitOfWork.DiscountRepository.UpdateAsync(id, discount);
            await _unitOfWork.SaveChangesAsync();
        }

        return _mapper.Map<DiscountDto>(discount);
    }

    public async Task<List<DiscountDto>> GetRoomsWithHighestDiscountsAsync(int topN)
    {
        var now = DateTime.UtcNow;

        var discounts = await _unitOfWork.DiscountRepository
            .GetAllAsync(query => query
                .Where(d => d.StartDateUtc <= now && d.EndDateUtc >= now)
                .OrderByDescending(d => d.Percentage)
                .Take(topN));

        var discountDtos = _mapper.Map<List<DiscountDto>>(discounts);
        foreach (var dto in discountDtos)
        {
            dto.IsActive = CalculateIsActive(dto.StartDateUtc, dto.EndDateUtc);
        }

        return discountDtos;
    }

    private bool CalculateIsActive(DateTime startDateUtc, DateTime endDateUtc)
    {
        var now = DateTime.UtcNow;
        return now >= startDateUtc && now <= endDateUtc;
    }
}
