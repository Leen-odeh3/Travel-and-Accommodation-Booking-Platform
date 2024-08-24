using HotelBookingPlatform.Domain.IRepositories;
namespace HotelBookingPlatform.Application.Helpers;
public class EntityValidator<T> where T : class
{
    private readonly IGenericRepository<T> _repository;
    public EntityValidator(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Validates the existence of an entity with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the entity to validate.</param>
    /// <returns>The entity if it exists.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the entity with the specified ID is not found.</exception>
    public async Task<T> ValidateExistenceAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
            throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} was not found.");

        return entity;
    }

    /// <summary>
    /// Validates that the provided entity is not null.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
    public void ValidateEntity(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), $"{typeof(T).Name} cannot be null.");
    }
}

