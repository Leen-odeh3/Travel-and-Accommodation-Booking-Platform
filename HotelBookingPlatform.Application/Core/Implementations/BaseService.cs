using HotelBookingPlatform.Domain.ILogger;

namespace HotelBookingPlatform.Application.Core.Implementations;
public abstract class BaseService<TEntity> where TEntity : class
{
    protected readonly IUnitOfWork<TEntity> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;

    public BaseService(IUnitOfWork<TEntity> unitOfWork, IMapper mapper, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
}
