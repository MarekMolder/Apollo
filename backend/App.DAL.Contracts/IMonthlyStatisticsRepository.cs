using App.DAL.DTO;
using Base.DAL.Contracts;

namespace App.DAL.Contracts;

public interface IMonthlyStatisticsRepository: IBaseRepository<MonthlyStatistics>
{
    Task<IEnumerable<MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    Task<Domain.Logic.MonthlyStatistics?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId);
    Task<IEnumerable<MonthlyStatistics?>> GetEnrichedMonthlyStatistics();
}