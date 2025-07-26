using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class StorageRoomBLLMapper : IMapper<App.BLL.DTO.StorageRoom, StorageRoom>
{
    private readonly CurrentStockBLLMapper _currentStockBLLMapper = new();
    private readonly ActionEntityBLLMapper _actionEntityBLLMapper = new();
    
    public StorageRoom? Map(DTO.StorageRoom? entity)
    {
        if (entity == null) return null;
        
        var res = new StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            Address = AddressBLLMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            EndedAt = entity.EndedAt,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockBLLMapper.Map(t)).ToList()!,
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!,
            
        };
        return res;
    }

    public DTO.StorageRoom? Map(StorageRoom? entity)
    {
         if (entity == null) return null;
        
        var res = new DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            AddressId = entity.AddressId,
            Address = AddressBLLMapper.MapSimple(entity.Address),
            
            AllowedRoles = entity.AllowedRoles?.ToList(),
            EndedAt = entity.EndedAt,
            
            CurrentStocks = entity.CurrentStocks?.Select(t => _currentStockBLLMapper.Map(t)).ToList()!,
            
            Actions = entity.Actions?.Select(t => _actionEntityBLLMapper.Map(t)).ToList()!,
            
        };
        return res;
    }
    
    public static StorageRoom? MapSimple(DTO.StorageRoom? entity)
    {
        if (entity == null) return null;

        return new StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
    
    public static DTO.StorageRoom? MapSimple(StorageRoom? entity)
    {
        if (entity == null) return null;

        return new DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
    }
}