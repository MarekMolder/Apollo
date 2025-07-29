using Base.BLL.Contracts;

namespace App.BLL.Contracts;

public interface IMonthlyStatisticsService : IBaseService<DTO.MonthlyStatistics>
{
    Task<IEnumerable<DTO.MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId);
}