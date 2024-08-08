using HotelBookingPlatform.Domain.Entities;
using HotelBookingPlatform.Domain.IRepositories;
using System.Linq.Expressions;
namespace HotelBookingPlatform.Domain.Abstracts;
public interface IInvoiceRecordRepository : IGenericRepository<InvoiceRecord>
{

    Task<IEnumerable<InvoiceRecord>> GetAllAsync(Expression<Func<InvoiceRecord, bool>> filter = null);

}
