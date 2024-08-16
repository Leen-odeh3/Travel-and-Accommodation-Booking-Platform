namespace HotelBookingPlatform.Application.Core.Implementations;
public abstract class BaseService<TEntity> where TEntity : class
{
    protected readonly IUnitOfWork<TEntity> _unitOfWork;
    protected readonly IMapper _mapper;
    public BaseService(IUnitOfWork<TEntity> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}
