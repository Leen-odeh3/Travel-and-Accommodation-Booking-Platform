using AutoMapper;
using HotelBookingPlatform.Domain.DTOs.InvoiceRecord;
using HotelBookingPlatform.Domain.Entities;

namespace HotelBookingPlatform.API.Profiles;
public class InvoiceRecordProfile : Profile
{
    public InvoiceRecordProfile()
    {
        CreateMap<InvoiceRecord, InvoiceCreateRequest>().ReverseMap();
    }
}
