using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.Contracts;

namespace App.DAL.EF.Mappers;

public class StorageRoomUOWMapper: IMapper<App.DAL.DTO.StorageRoom, App.Domain.Logic.StorageRoom>
{
    private readonly StorageRoomInInventoryUOWMapper _storageRoomInInventoryUOWMapper = new();
    private readonly CurrentStockUOWMapper _currentStockUOWMapper = new();
    private readonly ActionEntityUOWMapper _actionEntityUOWMapper = new();
    public StorageRoom? Map(Domain.Logic.StorageRoom? entity)
    {
        if (entity == null) return null;
        
        var res = new StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
            
            StorageRoomInInventories = entity.StorageRoomInInventories?.Select(t => _storageRoomInInventoryUOWMapper.Map(t)).ToList()!,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockUOWMapper.Map(t)).ToList()!,
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!,
            
        };
        return res;
    }

    public Domain.Logic.StorageRoom? Map(StorageRoom? entity)
    {
                if (entity == null) return null;
        
        var res = new Domain.Logic.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
            
            StorageRoomInInventories = entity.StorageRoomInInventories?.Select(t => _storageRoomInInventoryUOWMapper.Map(t)).ToList()!,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockUOWMapper.Map(t)).ToList()!,
            
            Actions = entity.Actions?.Select(t => _actionEntityUOWMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    public static StorageRoom? MapSimple(Domain.Logic.StorageRoom? entity)
    {
        if (entity == null) return null;

        return new StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }

    public static Domain.Logic.StorageRoom? MapSimple(StorageRoom? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
        };
    }
}
