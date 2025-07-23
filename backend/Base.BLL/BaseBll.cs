using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL;

public class BaseBll<TUOW> : IBaseBLL
where TUOW: IBaseUOW
{
    protected readonly TUOW BLLUOW;

    public BaseBll(TUOW uow)
    {
        BLLUOW = uow;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await BLLUOW.SaveChangesAsync();
    }
}