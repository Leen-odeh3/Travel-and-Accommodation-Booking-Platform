using System.Linq.Expressions;
namespace HotelBookingPlatform.Infrastructure.Test.TestRepositories;
public class InvoiceRecordRepositoryTest
{
    private readonly InvoiceRecordRepository _sut;
    private readonly InMemoryDbContext _context;
    private readonly IFixture _fixture;

    public InvoiceRecordRepositoryTest()
    {
        _context = new InMemoryDbContext();
        _sut = new InvoiceRecordRepository(_context);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnAllInvoiceRecords()
    {
        // Arrange
        var invoiceRecords = _fixture.Build<InvoiceRecord>()
            .CreateMany(5) 
            .ToList();

        _context.InvoiceRecords.AddRange(invoiceRecords);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count()); 
    }

    [Fact]
    public async Task GetAllAsync_WithFilter_ShouldReturnFilteredInvoiceRecords()
    {
        // Arrange
        var invoiceRecords = _fixture.Build<InvoiceRecord>()
            .With(ir => ir.PriceAtBooking, 100)
            .CreateMany(3)
            .ToList();

        var otherInvoiceRecords = _fixture.Build<InvoiceRecord>()
            .With(ir => ir.PriceAtBooking, 200)
            .CreateMany(2) 
            .ToList();

        _context.InvoiceRecords.AddRange(invoiceRecords);
        _context.InvoiceRecords.AddRange(otherInvoiceRecords);
        await _context.SaveChangesAsync();

        Expression<Func<InvoiceRecord, bool>> filter = ir => ir.PriceAtBooking == 100;

        // Act
        var result = await _sut.GetAllAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.All(result, ir => Assert.Equal(100, ir.PriceAtBooking));
    }
}
