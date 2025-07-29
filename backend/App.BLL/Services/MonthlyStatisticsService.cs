using App.BLL.Contracts;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class MonthlyStatisticsService : BaseService<MonthlyStatistics, DAL.DTO.MonthlyStatistics, IMonthlyStatisticsRepository>, IMonthlyStatisticsService
{
    private readonly IAppUOW _uow;
    private readonly IMapper<MonthlyStatistics, DAL.DTO.MonthlyStatistics> _dalToBLLMapper;
    public MonthlyStatisticsService(IAppUOW serviceUow, IMapper<MonthlyStatistics, DAL.DTO.MonthlyStatistics> mapper) : base(serviceUow, serviceUow.MonthlyStatisticsRepository, mapper)
    {
        _dalToBLLMapper = mapper;
        _uow = serviceUow;
    }
    
    public async Task<IEnumerable<MonthlyStatistics?>> GetByStorageRoomIdAsync(Guid storageRoomId)
    {
        var res = await ServiceRepository.GetByStorageRoomIdAsync(storageRoomId);
        return res.Select(u => _dalToBLLMapper.Map(u));
    }
}