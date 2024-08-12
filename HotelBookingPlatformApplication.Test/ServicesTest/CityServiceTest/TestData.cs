namespace HotelBookingPlatformApplication.Test.ServicesTest.CityServiceTest;
public static class TestData
{
    public static CityCreateRequest CityCreateRequest => new CityCreateRequest
    {
        Name = "Nablus",
        Country = "Palestine",
        Description = "A beautiful city in the northern West Bank."
    };

    public static City City => new City
    {
        CityID = 1,
        Name = "Nablus",
        Country = "Palestine",
        Description = "A beautiful city in the northern West Bank."
    };

    public static CityResponseDto CityResponseDto => new CityResponseDto
    {
        CityID = 1,
        Name = "Nablus",
        Country = "Palestine",
        Description = "A beautiful city in the northern West Bank."
    };

    public static List<City> Cities => new List<City>
{
    new City { CityID = 1, Name = "Nablus", Country = "Palestine", Description = "A beautiful city in the northern West Bank.", VisitCount = 0 },
    new City { CityID = 2, Name = "Ramallah", Country = "Palestine", Description = "The administrative capital of Palestine.", VisitCount = 0 }
};

    public static List<CityResponseDto> CityResponseDtos => new List<CityResponseDto>
{
    new CityResponseDto { CityID = 1, Name = "Nablus", Country = "Palestine", Description = "A beautiful city in the northern West Bank." },
    new CityResponseDto { CityID = 2, Name = "Ramallah", Country = "Palestine", Description = "The administrative capital of Palestine." }
};

    public static List<Hotel> Hotels => new List<Hotel>
{
    new Hotel { HotelId = 1, Name = "Hôtel Ritz Paris", Description = "Luxury hotel in Paris" },
    new Hotel { HotelId = 2, Name = "Le Meurice", Description = "5-star hotel with a view" },
    new Hotel { HotelId = 3, Name = "Shangri-La Paris", Description = "Elegant hotel in the heart of Paris" }
};

    public static List<HotelBasicResponseDto> HotelDtos => new List<HotelBasicResponseDto>
{
    new HotelBasicResponseDto { Name = "Hôtel Ritz Paris", Description = "Luxury hotel in Paris" },
    new HotelBasicResponseDto { Name = "Le Meurice", Description = "5-star hotel with a view" },
    new HotelBasicResponseDto { Name = "Shangri-La Paris", Description = "Elegant hotel in the heart of Paris" }
};
}
