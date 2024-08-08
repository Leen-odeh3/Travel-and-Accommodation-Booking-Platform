using HotelBookingPlatform.Domain.Abstracts;
using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Infrastructure.Data;
using HotelBookingPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
