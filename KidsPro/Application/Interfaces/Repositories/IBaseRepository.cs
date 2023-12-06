using System.Linq.Expressions;
using Application.Dtos.Response;

namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeProperties = "",
        bool disableTracking = false
    );

    Task<PagingResponse<T>> GetPaginateAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        int? page,
        int? size,
        string includeProperties = "",
        bool disableTracking = false
    );

    Task<T?> GetByIdAsync(object id, string? includeProperties = null, bool disableTracking = false);

    Task<List<T>> GetAllAsync();

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, T? entityFilter = null,
        string? includeProperties = null);

    Task AddAsync(T entity);

    void Update(T entity);

    Task DeleteByIdAsync(object id);

    Task<bool> ExistById(object id);

    Task<T> AddAsyncReturnEntity(T entity);
}