using App.DAL.DTO;
using Base.BLL.Contracts;
using Base.Contracts;

namespace App.BLL.Mappers;
 
public class StockAuditBLLMapper : IMapper<App.BLL.DTO.StockAudit, App.DAL.DTO.StockAudit>
{
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    public StockAudit? Map(DTO.StockAudit? entity)
    {
        if (entity == null) return null;
        
        var res = new StockAudit()
        {
            Id = entity.Id,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
           
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
            
        };
        return res;
    }

    public DTO.StockAudit? Map(StockAudit? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.StockAudit()
        {
            Id = entity.Id,
            
            StorageRoomId = entity.StorageRoomId,
            StorageRoom = StorageRoomBLLMapper.MapSimple(entity.StorageRoom),
           
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!
            
        };
        return res;
    }
    
    public static StockAudit? MapSimple(DTO.StockAudit? entity)
    {
        if (entity == null) return null;

        return new StockAudit()
        {
            Id = entity.Id,
            StorageRoomId = entity.StorageRoomId,
        };
    }
    
    public static DTO.StockAudit? MapSimple(StockAudit? entity)
    {
        if (entity == null) return null;

        return new DTO.StockAudit()
        {
            Id = entity.Id,
            StorageRoomId = entity.StorageRoomId,
        };
    }
}