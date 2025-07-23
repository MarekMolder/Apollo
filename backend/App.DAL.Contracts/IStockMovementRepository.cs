using App.Resources.Domain;
using Base.DAL.Contracts;
using StockMovement = App.Domain.Logic.StockMovement;

namespace App.DAL.Contracts;

public interface IStockMovementRepository: IBaseRepository<App.DAL.DTO.StockMovement>
{
    
}