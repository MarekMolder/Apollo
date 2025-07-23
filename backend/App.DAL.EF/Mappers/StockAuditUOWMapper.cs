using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class StockAuditUOWMapper: IMapper<App.DAL.DTO.StockAudit, App.Domain.Logic.StockAudit>
{
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    public StockAudit? Map(Domain.Logic.StockAudit? entity)
    {
        if (entity == null) return null;
        
        var res = new StockAudit()
        {
            Id = entity.Id,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
           
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
            
        };
        return res;
    }

    public Domain.Logic.StockAudit? Map(StockAudit? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.StockAudit()
        {
            Id = entity.Id,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomUOWMapper.MapSimple(entity.StorageRoom),
           
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!
            
        };
        return res;
    }
    
    public static StockAudit? MapSimple(Domain.Logic.StockAudit? entity)
    {
        if (entity == null) return null;

        return new StockAudit()
        {
            Id = entity.Id,
            StorageRoomId = entity.StorageRoomId,
        };
    }

    public static Domain.Logic.StockAudit? MapSimple(StockAudit? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.StockAudit()
        {
            Id = entity.Id,
            StorageRoomId = entity.StorageRoomId,
        };
    }
}
