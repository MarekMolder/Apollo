namespace Base.DAL.Contracts;

/// <summary>
/// Base interface for Unit of Work pattern.
/// </summary>
public interface IBaseUow
{
    /// <summary>
    /// Asynchronously saves all pending changes to the database.
    /// </summary>
    public Task<int> SaveChangesAsync();
}

