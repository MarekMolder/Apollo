using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class ActionTypeEntityService : BaseService<App.BLL.DTO.ActionTypeEntity, App.DAL.DTO.ActionTypeEntity, App.DAL.Contracts.IActionTypeEntityRepository>, IActionTypeEntityService
{
    public ActionTypeEntityService(IAppUOW serviceUow, 
        ActionTypeEntityBLLMapper bllMapper) : base(serviceUow, serviceUow.ActionTypeEntityRepository, bllMapper)
    {
    }
}