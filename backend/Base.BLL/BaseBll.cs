using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL;

/// <summary>
/// Base class for all Business Logic Layer (BLL) services.
/// Provides common functionality, such as saving changes via the Unit of Work.
/// </summary>
public class BaseBll<TUow> : IBaseBll
where TUow: IBaseUow
{
    protected readonly TUow Blluow;

    public BaseBll(TUow uow)
    {
        Blluow = uow;
    }
    
    /// <summary>
    /// Saves all changes made through the Unit of Work to the underlying data store.
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await Blluow.SaveChangesAsync();
    }
}