using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class StockAuditService : BaseService<App.BLL.DTO.StockAudit, App.DAL.DTO.StockAudit, App.DAL.Contracts.IStockAuditRepository>, IStockAuditService
{
    public StockAuditService(IAppUOW serviceUow, StockAuditBLLMapper bllMapper) : base(serviceUow, serviceUow.StockAuditRepository, bllMapper)
    {
    }
}