namespace HotelBookingPlatformInfrastructure.Test.TestRepositories.CityRepositoryTests;
public static class TestData
{
    public static City CreateCity(string name, string description, string country, string postOffice, int visitCount)
    {
        return new City
        {
            Name = name,
            Description = description,
            Country = country,
            PostOffice = postOffice,
            VisitCount = visitCount
        };
    }

    public static List<City> GetTestCities()
    {
        return new List<City>
            {
                CreateCity("Rome", "Historical city in Italy.", "Italy", "00100", 100),
                CreateCity("Milan", "Fashion capital of Italy.", "Italy", "20100", 150),
                CreateCity("Florence", "City known for its art and architecture.", "Italy", "50100", 200),
                CreateCity("Naples", "Historic city in southern Italy.", "Italy", "80100", 250),
                CreateCity("Venice", "City of canals and gondolas.", "Italy", "30100", 50)
            };
    }
}

