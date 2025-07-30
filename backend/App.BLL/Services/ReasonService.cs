using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing Reasons.
/// </summary>
public class ReasonService : BaseService<BLL.DTO.Reason, DAL.DTO.Reason, IReasonRepository>, IReasonService
{
    public ReasonService(IAppUOW serviceUow, ReasonBllMapper bllMapperReasons) : base(serviceUow, serviceUow.ReasonRepository, bllMapperReasons)
    {
    }
}