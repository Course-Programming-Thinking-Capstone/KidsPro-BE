using System.Linq.Expressions;
using Application.Dtos.Response;
using Application.ErrorHandlers;
using Application.Interfaces.Repositories.Generic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Generic;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger<BaseRepository<T>> _logger;

    public BaseRepository(AppDbContext context, ILogger<BaseRepository<T>> logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _logger = logger;
    }


    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? includeProperties = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = _dbSet;

        try
        {
            if (disableTracking)
            {
                query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when filter data of {typeof(T)} entity.");
            throw;
        }
    }

    public async Task<PagingResponse<T>> GetPaginateAsync(Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, int? page, int? size, string? includeProperties = null,
        bool disableTracking = false)
    {
        IQueryable<T> query = _dbSet;
        var result = new PagingResponse<T>();

        try
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            result.TotalRecords = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }


            if (page.HasValue && size.HasValue)
            {
                if (page.Value <= 0)
                {
                    throw new BadRequestException("Page must be greater than 0.");
                }

                if (size.Value <= 0)
                {
                    throw new BadRequestException("Size must be greater than 0.");
                }

                query = query.Skip((page.Value - 1) * size.Value).Take(size.Value);
                result.Results = await query.ToListAsync();
                result.TotalPages = (int)Math.Ceiling((double)result.TotalRecords / size.Value);
            }
            else
            {
                result.Results = await query.ToListAsync();
            }

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when filter data of {typeof(T)} entity.");
            throw new Exception(e.Message);
        }
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteByIdAsync(object id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<bool> ExistById(object id)
    {
        return await GetByIdAsync(id)
            .ContinueWith(t => t.Result == null);
    }
}