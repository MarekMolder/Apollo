using Base.BLL.Contracts;

namespace App.BLL.Contracts;

/// <summary>
/// Business logic contract for managing MonthlyStatistics operations.
/// </summary>
public interface IMonthlyStatisticsService : IBaseService<BLL.DTO.MonthlyStatistics>
{
    /// <summary>
    /// Retrieves MonthlyStatistics entries filtered by the specified storage room.
    /// </summary>
    Task<IEnumerable<BLL.DTO.MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    
    /// <summary>
    /// Retrieves MonthlyStatistics enriched with related data.
    /// </summary>
    Task<IEnumerable<BLL.DTO.MonthlyStatistics?>> GetEnrichedMonthlyStatistics();
}
