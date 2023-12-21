using System.Linq.Expressions;
using Application.Dtos.Response;
using Application.Dtos.Response.Paging;

namespace Application.Interfaces.Repositories.Generic;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string? includeProperties = null,
        bool disableTracking = false
    );

    IQueryable<T> GetAll();

    Task<PagingResponse<T>> GetPaginateAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        int? page,
        int? size,
        string? includeProperties = null,
        bool disableTracking = false
    );

    // Task<T?> GetByIdAsync(object id, string? includeProperties = null, bool disableTracking = false);
    Task<T?> GetByIdAsync(object id);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);

    Task DeleteByIdAsync(object id);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    Task<bool> ExistById(object id);
}