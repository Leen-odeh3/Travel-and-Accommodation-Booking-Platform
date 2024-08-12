namespace HotelBookingPlatform.API.Profiles;
public class InvoiceRecordProfile : Profile
{
    public InvoiceRecordProfile()
    {
        CreateMap<InvoiceRecord, InvoiceCreateRequest>().ReverseMap();

        CreateMap<InvoiceRecord, InvoiceResponseDto>();
    }
}
