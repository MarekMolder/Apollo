using Base.DAL.Contracts;

namespace App.DAL.Contracts;

/// <summary>
/// Repository interface for handling data access related to MonthlyStatistcs.
/// </summary>
public interface IMonthlyStatisticsRepository: IBaseRepository<DAL.DTO.MonthlyStatistics>
{
    /// <summary>
    /// Retrieves MonthlyStatistics entries filtered by the specified storage room.
    /// </summary>
    Task<IEnumerable<DAL.DTO.MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId);
    
    /// <summary>
    /// Finds a MonthlyStatistics domain model by product and storage room combination.
    /// </summary>
    Task<Domain.Logic.MonthlyStatistics?> FindByProductAndStorageAsync(Guid productId, Guid storageRoomId);
    
    /// <summary>
    /// Retrieves MonthlyStatistics enriched with related data.
    /// </summary>
    Task<IEnumerable<DAL.DTO.MonthlyStatistics?>> GetEnrichedMonthlyStatistics();
}