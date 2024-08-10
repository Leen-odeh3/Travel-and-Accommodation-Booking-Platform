namespace HotelBookingPlatform.Infrastructure.Implementation;
public class InvoiceRecordRepository : GenericRepository<InvoiceRecord>, IInvoiceRecordRepository
{
    public InvoiceRecordRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
    public async Task<IEnumerable<InvoiceRecord>> GetAllAsync(Expression<Func<InvoiceRecord, bool>> filter = null)
    {
        IQueryable<InvoiceRecord> query = _appDbContext.Set<InvoiceRecord>();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }
}
