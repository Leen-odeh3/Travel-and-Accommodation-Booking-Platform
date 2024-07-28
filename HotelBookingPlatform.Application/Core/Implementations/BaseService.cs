using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.IServices;
using HotelBookingPlatform.Domain;
using AutoMapper;
namespace HotelBookingPlatform.Application.Core.Implementations;
public abstract class BaseService<TEntity> where TEntity : class
{
    protected readonly IUnitOfWork<TEntity> _unitOfWork;
    protected readonly IMapper _mapper;
    protected readonly ResponseHandler _responseHandler;

    protected BaseService(IUnitOfWork<TEntity> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _responseHandler = responseHandler;
    }
}