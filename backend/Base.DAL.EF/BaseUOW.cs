using Base.DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

/// <summary>
/// Base Unit of Work implementation for Entity Framework DbContext.
/// Provides a single point to commit changes to the database.
/// </summary>
public class BaseUow<TDbContext> : IBaseUow
where TDbContext : DbContext
{
    protected readonly TDbContext UowDbContext;

    public BaseUow(TDbContext uowDbContext)
    {
        UowDbContext = uowDbContext;
    }
    
    /// <summary>
    /// Asynchronously commits all changes made in the current unit of work to the database.
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await UowDbContext.SaveChangesAsync();
    }
}