namespace Base.BLL.Contracts;

/// <summary>
/// Defines the base interface for the Business Logic Layer (BLL).
/// </summary>
public interface IBaseBll
{
    /// <summary>
    /// Saves all pending changes across the BLL to the database.
    /// </summary>
    public Task<int> SaveChangesAsync();
}