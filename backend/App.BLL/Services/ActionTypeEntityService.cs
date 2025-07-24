using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class ActionTypeEntityService : BaseService<ActionTypeEntity, DAL.DTO.ActionTypeEntity, IActionTypeEntityRepository>, IActionTypeEntityService
{
    public ActionTypeEntityService(IAppUOW serviceUow, 
        ActionTypeEntityBLLMapper bllMapper) : base(serviceUow, serviceUow.ActionTypeEntityRepository, bllMapper)
    {
    }
}