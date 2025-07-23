using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using App.DAL.DTO;
using Base.BLL;
using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace App.BLL.Services;

public class StockMovementService : BaseService<App.BLL.DTO.StockMovement, App.DAL.DTO.StockMovement, App.DAL.Contracts.IStockMovementRepository>, IStockMovementService
{
    public StockMovementService(IAppUOW serviceUow, StockMovementBLLMapper bllMapper) : base(serviceUow, serviceUow.StockMovementRepository, bllMapper)
    {
    }
}