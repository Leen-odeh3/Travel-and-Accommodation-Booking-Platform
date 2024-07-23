using Microsoft.EntityFrameworkCore;
namespace HotelBookingPlatform.Infrastructure.Data;
public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
    {
        
    }
}
