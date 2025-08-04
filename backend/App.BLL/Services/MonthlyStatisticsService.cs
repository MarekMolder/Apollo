using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

/// <summary>
/// Business logic service for managing MonthlyStatistics.
/// Provides access to filtered and enriched monthly data related to product removal statistics.
/// </summary>
public class MonthlyStatisticsService : BaseService<BLL.DTO.MonthlyStatistics, DAL.DTO.MonthlyStatistics, IMonthlyStatisticsRepository>, IMonthlyStatisticsService
{
    private readonly IAppUOW _uow;
    
    // Maps between DAL.DTO and BLL.DTO MonthlyStatistics
    private readonly IMapper<BLL.DTO.MonthlyStatistics, DAL.DTO.MonthlyStatistics> _dalBllMapperMonthlyStatistics;
    public MonthlyStatisticsService(IAppUOW serviceUow, IMapper<BLL.DTO.MonthlyStatistics, DAL.DTO.MonthlyStatistics> mapperMonthlyStatistics) : base(serviceUow, serviceUow.MonthlyStatisticsRepository, mapperMonthlyStatistics)
    {
        _dalBllMapperMonthlyStatistics = mapperMonthlyStatistics;
        _uow = serviceUow;
    }
    
    /// <summary>
    /// Retrieves all MonthlyStatistics entries for a given storage room.
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId)
    {
        var res = await ServiceRepository.GetByStorageRoomIdAsync(storageRoomId);
        return res.Select(u => _dalBllMapperMonthlyStatistics.Map(u));
    }
    
    /// <summary>
    /// Returns MonthlyStatistics enriched with related data (joins from DB).
    /// </summary>
    public async Task<IEnumerable<BLL.DTO.MonthlyStatistics?>> GetEnrichedMonthlyStatistics()
    {
        var res = await ServiceRepository.GetEnrichedMonthlyStatistics();
        return res.Select(u => _dalBllMapperMonthlyStatistics.Map(u));
    }
}