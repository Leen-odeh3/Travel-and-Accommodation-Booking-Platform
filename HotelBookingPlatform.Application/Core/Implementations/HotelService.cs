using AutoMapper;
using HotelBookingPlatform.Application.Core.Abstracts;
using HotelBookingPlatform.Domain.DTOs.Hotel;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain;
using System.Linq.Expressions;
using HotelBookingPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using KeyNotFoundException = HotelBookingPlatform.Domain.Exceptions.KeyNotFoundException;
using HotelBookingPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;
using HotelBookingPlatform.Domain.IServices;
using InvalidOperationException = HotelBookingPlatform.Domain.Exceptions.InvalidOperationException;

namespace HotelBookingPlatform.Application.Core.Implementations;
public class HotelService : BaseService<Hotel>, IHotelService
{
    private readonly IFileService _fileService;

    public HotelService(IUnitOfWork<Hotel> unitOfWork, IMapper mapper,IFileService fileService)
         : base(unitOfWork, mapper)
    {
        _fileService = fileService;
    }
    public async Task<IEnumerable<HotelResponseDto>> GetHotels(string hotelName, string description, int pageSize, int pageNumber)
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
        var hotels = await _unitOfWork.HotelRepository.GetAllAsyncPagenation(filter, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        if (!hotelDtos.Any())
            throw new NotFoundException("No hotels found matching the criteria.");

        return hotelDtos;
    }
    public async Task<HotelResponseDto> GetHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);

        if (hotel is null)
            throw new NotFoundException("Hotel not found");

        var hotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return hotelDto;
    }

    public async Task<ActionResult<HotelResponseDto>> CreateHotel(HotelCreateRequest request)
    {
        var hotel = _mapper.Map<Hotel>(request);
        await _unitOfWork.HotelRepository.CreateAsync(hotel);
        await _unitOfWork.SaveChangesAsync();

        var createdHotelDto = _mapper.Map<HotelResponseDto>(hotel);
        return new CreatedAtActionResult(nameof(GetHotel), "Hotels", new { id = createdHotelDto.HotelId }, createdHotelDto);
    }
    public async Task<HotelResponseDto> UpdateHotelAsync(int id, HotelResponseDto request)
    {
        var existingHotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (existingHotel is null)
        {
            throw new KeyNotFoundException("Hotel not found");
        }
        _mapper.Map(request, existingHotel);

        await _unitOfWork.HotelRepository.UpdateAsync(id, existingHotel);
        await _unitOfWork.SaveChangesAsync();
        var updatedHotelDto = _mapper.Map<HotelResponseDto>(existingHotel);
        return updatedHotelDto;
    }
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _unitOfWork.HotelRepository.GetByIdAsync(id);
        if (hotel is null)
        {
            throw new KeyNotFoundException("Hotel not found");
        }

        await _unitOfWork.HotelRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return new OkObjectResult(new { message = "Hotel successfully deleted." });
    }
    public async Task<IEnumerable<HotelResponseDto>> SearchHotel(string name, string desc, int pageSize, int pageNumber)
    {
        var hotels = await _unitOfWork.HotelRepository.SearchCriteria(name, desc, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelResponseDto>>(hotels);

        return hotelDtos;
    }


}


