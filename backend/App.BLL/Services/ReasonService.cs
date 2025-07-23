using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class ReasonService : BaseService<App.BLL.DTO.Reason, App.DAL.DTO.Reason, App.DAL.Contracts.IReasonRepository>, IReasonService
{
    public ReasonService(IAppUOW serviceUow, ReasonBLLMapper bllMapper) : base(serviceUow, serviceUow.ReasonRepository, bllMapper)
    {
    }
}