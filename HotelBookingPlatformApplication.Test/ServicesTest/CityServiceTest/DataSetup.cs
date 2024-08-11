using HotelBookingPlatform.Domain.DTOs.City;
using HotelBookingPlatform.Domain.DTOs.Hotel;

namespace HotelBookingPlatformApplication.Test.ServicesTest.CityServiceTest;
public static class DataSetup
{
    public static City CreateSampleCity()
    {
        return new City
        {
            CityID = 1,
            Name = "Sample City",
            Country = "Sample Country",
            PostOffice = "12345",
            CreatedAtUtc = DateTime.UtcNow,
            Description = "A sample city for testing.",
            VisitCount = 10,
            Hotels = new List<Hotel>
                {
                    new Hotel
                    {
                        HotelId = 1,
                        Name = "Sample Hotel",
                        CityID = 1
                    }
                }
        };
    }

    public static CityCreateRequest CreateSampleCityCreateRequest()
    {
        return new CityCreateRequest
        {
            Name = "Sample City",
            Country = "Sample Country",
            PostOffice = "12345",
            Description = "A sample city for testing."
        };
    }

    public static CityResponseDto CreateSampleCityResponseDto()
    {
        return new CityResponseDto
        {
            CityID = 1,
            Name = "Sample City",
            Country = "Sample Country",
            PostOffice = "12345",
            CreatedAtUtc = DateTime.UtcNow,
            Description = "A sample city for testing.",
        };
    }

    public static CityWithHotelsResponseDto CreateSampleCityWithHotelsResponseDto()
    {
        return new CityWithHotelsResponseDto
        {
            CityID = 1,
            Name = "Sample City",
            Country = "Sample Country",
            PostOffice = "12345",
            CreatedAtUtc = DateTime.UtcNow,
            Description = "A sample city for testing.",
            Hotels = new List<HotelResponseDto>
                {
                    new HotelResponseDto
                    {
                        HotelId = 1,
                        Name = "Sample Hotel"
                    }
                }
        };
    }

    public static HotelCreateRequest CreateSampleHotelCreateRequest()
    {
        return new HotelCreateRequest
        {
            Name = "New Sample Hotel",
            // Add other properties as needed
        };
    }

    public static IEnumerable<City> CreateSampleCities(int count)
    {
        var cities = new List<City>();
        for (int i = 1; i <= count; i++)
        {
            cities.Add(new City
            {
                CityID = i,
                Name = $"City {i}",
                Country = $"Country {i}",
                PostOffice = $"{10000 + i}",
                CreatedAtUtc = DateTime.UtcNow,
                Description = $"Description for City {i}.",
                VisitCount = i
            });
        }
        return cities;
    }

    public static IEnumerable<CityResponseDto> CreateSampleCityResponseDtos(int count)
    {
        var cityDtos = new List<CityResponseDto>();
        for (int i = 1; i <= count; i++)
        {
            cityDtos.Add(new CityResponseDto
            {
                CityID = i,
                Name = $"City {i}",
                Country = $"Country {i}",
                PostOffice = $"{10000 + i}",
                CreatedAtUtc = DateTime.UtcNow,
                Description = $"Description for City {i}.",
            });
        }
        return cityDtos;
    }
}
