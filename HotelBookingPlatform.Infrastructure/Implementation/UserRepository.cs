namespace HotelBookingPlatform.Infrastructure.Implementation;
public class UserRepository : GenericRepository<LocalUser>, IUserRepository
{
    private readonly UserManager<LocalUser> _userManager;
    private readonly ILog _logger;
    public UserRepository(UserManager<LocalUser> userManager, AppDbContext appDbContext, ILog logger) :base(appDbContext, logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger;

    }

    public async Task<LocalUser> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Invalid email", nameof(email));
        }

        return await _userManager.FindByEmailAsync(email);
    }
    public async Task<LocalUser> GetByIdAsync(string userId)
    {
        return await _appDbContext.LocalUsers
            .AsNoTracking()  
            .FirstOrDefaultAsync(user => user.Id == userId);
    }
}
