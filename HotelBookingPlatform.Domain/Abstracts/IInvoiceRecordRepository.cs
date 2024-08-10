namespace HotelBookingPlatform.Domain.Abstracts;
public interface IInvoiceRecordRepository : IGenericRepository<InvoiceRecord>
{

    Task<IEnumerable<InvoiceRecord>> GetAllAsync(Expression<Func<InvoiceRecord, bool>> filter = null);

}
