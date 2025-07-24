using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;

namespace App.BLL.Services;

public class ReasonService : BaseService<Reason, DAL.DTO.Reason, IReasonRepository>, IReasonService
{
    public ReasonService(IAppUOW serviceUow, ReasonBLLMapper bllMapper) : base(serviceUow, serviceUow.ReasonRepository, bllMapper)
    {
    }
}