﻿using Microsoft.EntityFrameworkCore;

namespace HotelBookingPlatform.Infrastructure.Implementation;
public class ImageRepository:GenericRepository<Image> , IImageRepository
{
    private readonly ILog _logger;
    public ImageRepository(AppDbContext context, ILog logger)
        : base(context, logger)
    {
        _logger = logger;
    }
    public async Task<Image> GetByPublicIdAsync(string publicId)
    {
        return await _appDbContext.Set<Image>().FirstOrDefaultAsync(img => img.PublicId == publicId);
    }
}
