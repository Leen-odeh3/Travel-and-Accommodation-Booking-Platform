namespace HotelBookingPlatform.Infrastructure.Implementation;
public class InvoiceRecordRepository : GenericRepository<InvoiceRecord>, IInvoiceRecordRepository
{
    private readonly ILog _logger;
    public InvoiceRecordRepository(AppDbContext context, ILog logger)
        : base(context, logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<InvoiceRecord>> GetAllAsync(Expression<Func<InvoiceRecord, bool>> filter = null)
    {
        IQueryable<InvoiceRecord> query = _appDbContext.Set<InvoiceRecord>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }
}
