using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing ActionTypeEntities.
/// </summary>
public class ActionTypeEntityService : BaseService<BLL.DTO.ActionTypeEntity, DAL.DTO.ActionTypeEntity, IActionTypeEntityRepository>, IActionTypeEntityService
{
    public ActionTypeEntityService(IAppUOW serviceUow, 
        ActionTypeEntityBllMapper bllMapperActionType) : base(serviceUow, serviceUow.ActionTypeEntityRepository, bllMapperActionType)
    {
    }
}
