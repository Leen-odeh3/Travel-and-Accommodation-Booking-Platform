using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain;
using HotelBookingPlatform.Domain.DTOs.RoomClass;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class RoomClassService : BaseService<RoomClass>, IRoomClassService
{
    public RoomClassService(IUnitOfWork<RoomClass> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }

}
