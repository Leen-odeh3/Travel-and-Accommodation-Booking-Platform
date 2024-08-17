namespace HotelBookingPlatform.Application.Extentions;
public static class ModuleApplicationDependencies
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<OwnerValidator>();
            fv.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>();
        });


        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomClassService, RoomClassService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IAmenityService, AmenityService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IHotelAmenitiesService, HotelAmenitiesService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IInvoiceRecordService, InvoiceRecordService>();
        services.AddScoped<IImageService, ImageService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IValidator<HotelCreateRequest>, HotelCreateRequestValidator>();

        return services;
    }
}

