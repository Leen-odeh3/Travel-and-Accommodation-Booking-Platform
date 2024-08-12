namespace HotelBookingPlatform.Domain.IServices;
public interface IRoleService
{
    Task<string> AddRoleAsync(AddRoleModel model);
}
