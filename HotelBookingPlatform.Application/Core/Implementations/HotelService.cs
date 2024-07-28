using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.Bases;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using System.Linq.Expressions;

namespace HotelBookingPlatform.Application.Core.Implementations;

public class HotelService : BaseService<Hotel>, IHotelService
{
    public HotelService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper, ResponseHandler responseHandler)
        : base(unitOfWork, mapper, responseHandler)
    {
    }

    public async Task<Response<IEnumerable<HotelResponseDto>>> GetHotels(string hotelName, string description, int pageSize, int pageNumber)
    {
        Expression<Func<Hotel, bool>> filter = null;

        if (!string.IsNullOrEmpty(hotelName) || !string.IsNullOrEmpty(description))
        {
            if (!string.IsNullOrEmpty(hotelName) && !string.IsNullOrEmpty(description))
            {
                filter = h => h.Name.Contains(hotelName) && h.Description.Contains(description);
            }
            else if (!string.IsNullOrEmpty(hotelName))
            {
                filter = h => h.Name.Contains(hotelName);
            }
            else if (!string.IsNullOrEmpty(description))
            {
                filter = h => h.Description.Contains(description);
            }
        }

        var hotels = await _unitOfWork.HotelRepository.GetAllAsync(filter, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        return hotelDtos.Any()
            ? _responseHandler.Success(hotelDtos)
            : _responseHandler.NotFound<IEnumerable<HotelResponseDto>>("No Hotels Found");
    }

    public async Task<Response<HotelResponseDto>> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);

        if (hotel is null)
            return _responseHandler.NotFound<HotelResponseDto>("Hotel not found");

        var hotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return _responseHandler.Success(hotelDto);
    }

    public async Task<Response<HotelResponseDto>> CreateHotel(HotelCreateRequest request)
    {
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        var createdHotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return _responseHandler.Created(createdHotelDto);
    }


    public async Task<Response<HotelResponseDto>> UpdateHotelAsync(int id, HotelResponseDto request)
    {
        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (existingHotel == null)
            return _responseHandler.NotFound<HotelResponseDto>("Hotel not found");

        _mapper.Map(request, existingHotel);
        await _unitOfWork.HotelRepository.UpdateAsync(id, existingHotel);
        await _unitOfWork.SaveChangesAsync();

        var updatedHotelDto = _mapper.Map<HotelResponseDto>(existingHotel);
        return _responseHandler.Success(updatedHotelDto);
    }



public async Task<Response<HotelResponseDto>> DeleteHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
            return _responseHandler.NotFound<HotelResponseDto>("Hotel not found");

        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return _responseHandler.Deleted<HotelResponseDto>("Hotel successfully deleted.");
    }

    public async Task<Response<IEnumerable<HotelResponseDto>>> SearchHotel(string name, string desc, int pageSize, int pageNumber)
    {
        var hotels = await _unitOfWork.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        return hotelDtos.Any()
            ? _responseHandler.Success(hotelDtos)
            : _responseHandler.NotFound<IEnumerable<HotelResponseDto>>("No Hotels Found");
    }
}

